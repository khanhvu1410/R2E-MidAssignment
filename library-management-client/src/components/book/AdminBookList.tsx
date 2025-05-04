import { useEffect, useState } from 'react';
import {
  Button,
  Card,
  List,
  message,
  Popconfirm,
  Space,
  Spin,
  Table,
  TableProps,
  Tag,
  Tooltip,
} from 'antd';
import {
  DeleteOutlined,
  EditOutlined,
  EyeOutlined,
  PlusOutlined,
} from '@ant-design/icons';
import BodyLayout from '../layout/BodyLayout';
import { Book } from '../../models/book';
import { deleteBookService, getPagedBooksService } from '../../api/bookService';
import { Link } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { useMediaQuery } from 'react-responsive';

const defaultQueryParameters = {
  pageIndex: 1,
  pageSize: 10,
  totalCount: 0,
  search: '',
};

const AdminBookList = () => {
  const breadcrumbItems = [{ title: 'Book' }];

  const columns: TableProps<Book>['columns'] = [
    {
      title: 'Title',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Author',
      dataIndex: 'author',
      key: 'author',
    },
    {
      title: 'ISBN',
      dataIndex: 'isbn',
      key: 'isbn',
    },
    {
      title: 'Publication Year',
      dataIndex: 'publicationYear',
      key: 'publicationYear',
      align: 'center',
    },
    {
      title: 'Category',
      dataIndex: 'categoryName',
      key: 'categoryName',
      align: 'center',
    },
    {
      title: 'Quantity',
      dataIndex: 'quantity',
      key: 'quantity',
      align: 'center',
      render: (quantity: number) => {
        let color = '';
        if (quantity > 0) {
          color = 'green';
        } else {
          color = 'red';
        }
        return <Tag color={color}>{quantity}</Tag>;
      },
    },
    {
      title: 'Actions',
      key: 'actions',
      align: 'center',
      width: 150,
      fixed: 'right',
      render: (_, record) => (
        <Space size="middle">
          <Tooltip title="View details">
            <Link
              to={PATH.admin.bookDetails.replace(':id', record.id.toString())}
            >
              <Button
                type="text"
                icon={<EyeOutlined />}
                aria-label={`View ${record.title}`}
              />
            </Link>
          </Tooltip>
          <Tooltip title="Edit">
            <Link to={PATH.admin.editBook.replace(':id', record.id.toString())}>
              <Button
                type="text"
                icon={<EditOutlined />}
                aria-label={`Edit ${record.title}`}
              />
            </Link>
          </Tooltip>
          <Tooltip title="Delete">
            <Popconfirm
              title="Delete this book?"
              onConfirm={() => {
                handleDeleteBook(record.id);
              }}
            >
              <Button
                type="text"
                icon={<DeleteOutlined />}
                aria-label={`Delete ${record.title}`}
                danger
              />
            </Popconfirm>
          </Tooltip>
        </Space>
      ),
    },
  ];

  const isSmallDevice = useMediaQuery({ maxWidth: 768 });
  const [books, setBooks] = useState<Book[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [bookQueryParameters, setBookQueryParameters] = useState(
    defaultQueryParameters
  );

  useEffect(() => {
    let isSetData = true;
    getPagedBooksService(
      bookQueryParameters.pageIndex,
      bookQueryParameters.pageSize
    )
      .then((response) => {
        if (isSetData) {
          setBooks(
            response.data.items.map((book: Book) => {
              return {
                ...book,
                key: book.id,
              };
            })
          );
          setBookQueryParameters((prev) => ({
            ...prev,
            totalCount: response.data.totalRecords,
          }));
        }
      })
      .catch((err) => {
        message.error(err.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
    return () => {
      isSetData = false;
    };
  }, [bookQueryParameters.pageIndex, bookQueryParameters.pageSize]);

  const handleDeleteBook = (id: number) => {
    deleteBookService(id)
      .then(() => {
        // Filter out the deleted book
        setBooks((prevBooks) => prevBooks.filter((book) => book.id !== id));

        // Update total count
        setBookQueryParameters((prev) => ({
          ...prev,
          totalCount: prev.totalCount - 1,
        }));

        // If the current page would be empty after deletion, go to previous page
        if (books.length === 1 && bookQueryParameters.pageIndex > 1) {
          setBookQueryParameters((prev) => ({
            ...prev,
            pageIndex: prev.pageIndex - 1,
          }));
        }
      })
      .catch((err) => {
        message.error(err.message);
      });
  };

  const handleOnPageChange = (page: number, pageSize: number) => {
    setIsLoading(true);
    setBookQueryParameters({
      ...bookQueryParameters,
      pageIndex: page,
      pageSize: pageSize,
    });
  };

  const handleOnPageSizeChange = (current: number, size: number) => {
    setIsLoading(true);
    setBookQueryParameters({
      ...bookQueryParameters,
      pageIndex: current,
      pageSize: size,
    });
  };

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Books List"
      createButton={
        <Link to={PATH.admin.createBook}>
          <Button type="primary" icon={<PlusOutlined />}>
            Create Book
          </Button>
        </Link>
      }
    >
      {isLoading && (
        <div
          style={{
            textAlign: 'center',
            padding: 40,
          }}
        >
          <Spin size="large" />
        </div>
      )}

      {isSmallDevice && !isLoading && (
        <List
          dataSource={books}
          pagination={{
            pageSize: bookQueryParameters.pageSize,
            current: bookQueryParameters.pageIndex,
            total: bookQueryParameters.totalCount,
            showSizeChanger: true,
            pageSizeOptions: [5, 10, 20, 50],
            showTotal: (total) => `Total ${total} books`,
            onChange: (page, pageSize) => {
              handleOnPageChange(page, pageSize);
            },
            onShowSizeChange: (current, size) => {
              handleOnPageSizeChange(current, size);
            },
          }}
          renderItem={(book) => (
            <Card
              title={book.title}
              size="small"
              style={{ marginBottom: 16 }}
              actions={[
                <Tooltip title="View details">
                  <Link
                    to={PATH.admin.bookDetails.replace(
                      ':id',
                      book.id.toString()
                    )}
                  >
                    <Button
                      type="text"
                      icon={<EyeOutlined />}
                      aria-label={`View ${book.title}`}
                    />
                  </Link>
                </Tooltip>,
                <Tooltip title="Edit">
                  <Link
                    to={PATH.admin.editBook.replace(':id', book.id.toString())}
                  >
                    <Button
                      type="text"
                      icon={<EditOutlined />}
                      onClick={() => book}
                      aria-label={`Edit ${book.title}`}
                    />
                  </Link>
                </Tooltip>,
                <Tooltip title="Delete">
                  <Popconfirm
                    title="Are you sure to delete this book?"
                    onConfirm={() => handleDeleteBook(book.id)}
                    okText="Yes"
                    cancelText="No"
                  >
                    <Button
                      danger
                      type="text"
                      icon={<DeleteOutlined />}
                      aria-label={`Delete ${book.title}`}
                    />
                  </Popconfirm>
                </Tooltip>,
              ]}
            >
              <p>
                <strong>Title:</strong> {book.title}
              </p>
              <p>
                <strong>Author:</strong> {book.author}
              </p>
              <p>
                <strong>ISBN:</strong> {book.isbn}
              </p>
              <p>
                <strong>Publication year:</strong> {book.publicationYear}
              </p>
              <p>
                <strong>Quantity:</strong> {book.quantity}
              </p>
              <p>
                <strong>Category name:</strong> {book.categoryName}
              </p>
            </Card>
          )}
        />
      )}

      {!isLoading && !isSmallDevice && (
        <Table<Book>
          columns={columns}
          dataSource={books}
          scroll={{ x: 'max-content' }}
          size="middle"
          bordered
          pagination={{
            pageSize: bookQueryParameters.pageSize,
            current: bookQueryParameters.pageIndex,
            total: bookQueryParameters.totalCount,
            showSizeChanger: true,
            pageSizeOptions: [5, 10, 20, 50],
            showTotal: (total) => `Total ${total} books`,
            onChange: (page, pageSize) => {
              handleOnPageChange(page, pageSize);
            },
            onShowSizeChange: (current, size) => {
              handleOnPageSizeChange(current, size);
            },
          }}
        />
      )}
    </BodyLayout>
  );
};

export default AdminBookList;

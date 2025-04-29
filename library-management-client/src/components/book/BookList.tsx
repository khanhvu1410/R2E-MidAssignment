import {
  Alert,
  Button,
  Card,
  List,
  Popconfirm,
  Space,
  Spin,
  Table,
  TableProps,
  Tag,
  Tooltip,
} from 'antd';
import { useEffect, useState } from 'react';
import { Book } from '../../models/book';

import {
  DeleteOutlined,
  EditOutlined,
  EyeOutlined,
  PlusOutlined,
} from '@ant-design/icons';
import { deleteBookService, getAllBooksService } from '../../api/bookService';
import { Link } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import BodyLayout from '../layout/BodyLayout';
import { useMediaQuery } from 'react-responsive';

const defaultQueryParameters = {
  pageIndex: 1,
  pageSize: 10,
  totalCount: 0,
  search: '',
};

const BookList = () => {
  const columns: TableProps<Book>['columns'] = [
    {
      title: 'ID',
      dataIndex: 'id',
      key: 'id',
      align: 'center',
      width: 80,
    },
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
      title: 'Published Date',
      dataIndex: 'publishedDate',
      key: 'publishedDate',
    },
    {
      title: 'Category',
      dataIndex: 'category',
      key: 'category',
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      align: 'center',
      fixed: 'right',
      render: (status) => (
        <Tag
          style={{ textTransform: 'capitalize' }}
          color={status === 'available' ? 'green' : 'red'}
        >
          {status}
        </Tag>
      ),
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
            <Button
              type="text"
              icon={<EyeOutlined />}
              aria-label={`View ${record.title}`}
            />
          </Tooltip>
          <Tooltip title="Edit">
            <Link to={PATH.editBook.replace(':id', record.id.toString())}>
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

  const [books, setBooks] = useState<Book[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [bookQueryParameters, setBookQueryParameters] = useState(
    defaultQueryParameters
  );

  useEffect(() => {
    let isSetData = true;
    const timeoutId = setTimeout(() => {
      getAllBooksService()
        .then((response) => {
          if (isSetData) {
            setBooks(
              response.data.map((book: Book) => {
                return {
                  ...book,
                  key: book.id,
                };
              })
            );
          }
        })
        .catch((err) => {
          setError(err.message);
        })
        .finally(() => {
          setIsLoading(false);
        });
    }, 1000);
    return () => {
      isSetData = false;
      clearTimeout(timeoutId);
    };
  }, [
    bookQueryParameters.pageIndex,
    bookQueryParameters.pageSize,
    bookQueryParameters.totalCount,
  ]);

  const handleDeleteBook = (id: number) => {
    deleteBookService(id)
      .then(() => {
        setBookQueryParameters({
          ...bookQueryParameters,
          totalCount: bookQueryParameters.totalCount - 1,
        });
      })
      .catch((err) => {
        window.alert(err.message);
      });
  };

  const handleOnPageChange = (page: number, pageSize: number) => {
    setBookQueryParameters({
      ...bookQueryParameters,
      pageIndex: page,
      pageSize: pageSize,
    });
  };

  const breadcrumbItems = [{ title: 'Book' }];
  const isSmallDevice = useMediaQuery({ maxWidth: 768 });

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Books List"
      createButton={
        <Link to={PATH.createBook}>
          <Button type="primary" icon={<PlusOutlined />}>
            Create Book
          </Button>
        </Link>
      }
    >
      <div>
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

        {error != null && (
          <Alert message="Error" description={error} type="error" showIcon />
        )}

        {isSmallDevice && !error && !isLoading && (
          <List
            dataSource={books}
            pagination={{
              pageSize: bookQueryParameters.pageSize,
              current: bookQueryParameters.pageIndex,
              showTotal: (total) => `Total ${total} books`,
              onChange: (page, pageSize) => {
                handleOnPageChange(page, pageSize);
              },
            }}
            renderItem={(book) => (
              <Card
                title={book.title}
                size="small"
                style={{ marginBottom: 16 }}
                actions={[
                  <Tooltip title="View details">
                    <Button
                      type="text"
                      icon={<EyeOutlined />}
                      aria-label={`View ${book.title}`}
                    />
                  </Tooltip>,
                  <Tooltip title="Edit">
                    <Link to={PATH.editBook.replace(':id', book.id.toString())}>
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
                  <strong>Published date:</strong> {book.publishedDate}
                </p>
                <p>
                  <strong>Catgory:</strong> {book.category}
                </p>
                <p>
                  <strong>Status:</strong> {book.status}
                </p>
              </Card>
            )}
          />
        )}

        {!isLoading && !error && !isSmallDevice && (
          <Table<Book>
            columns={columns}
            dataSource={books}
            scroll={{ x: 'max-content' }}
            size="middle"
            bordered
            pagination={{
              pageSize: bookQueryParameters.pageSize,
              current: bookQueryParameters.pageIndex,
              showTotal: (total) => `Total ${total} books`,
              onChange: (page, pageSize) => {
                handleOnPageChange(page, pageSize);
              },
            }}
          />
        )}
      </div>
    </BodyLayout>
  );
};

export default BookList;

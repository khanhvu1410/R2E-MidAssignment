import { useEffect, useState } from 'react';
import {
  Button,
  Card,
  Checkbox,
  List,
  message,
  Space,
  Spin,
  Table,
  TableProps,
  Tag,
  Tooltip,
} from 'antd';
import { EyeOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import BodyLayout from '../layout/BodyLayout';
import { Book } from '../../models/book';
import { getPagedBooksService } from '../../api/bookService';
import { Link } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { useMediaQuery } from 'react-responsive';
import {
  createBorrowingRequestService,
  getBorrowingRequestsThisMonthService,
} from '../../api/borrowingRequestService';
import { BorrowingRequest } from '../../models/borrowingRequest';

const defaultQueryParameters = {
  pageIndex: 1,
  pageSize: 10,
  totalCount: 0,
  search: '',
};

const UserBookList = () => {
  const breadcrumbItems = [{ title: 'Book' }];
  const [selectedBooks, setSelectedBooks] = useState<number[]>([]);

  const columns: TableProps<Book>['columns'] = [
    {
      title: 'Select',
      key: 'select',
      align: 'center',
      width: 80,
      fixed: 'right',
      render: (_, record) => (
        <Checkbox
          checked={selectedBooks.includes(record.id)}
          onChange={(e) => {
            if (e.target.checked) {
              setSelectedBooks([...selectedBooks, record.id]);
            } else {
              setSelectedBooks(selectedBooks.filter((id) => id !== record.id));
            }
          }}
        />
      ),
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
      width: 100,
      fixed: 'right',
      render: (_, record) => (
        <Space size="middle">
          <Tooltip title="View details">
            <Link
              to={PATH.user.bookDetails.replace(':id', record.id.toString())}
            >
              <Button
                type="text"
                icon={<EyeOutlined />}
                aria-label={`View ${record.title}`}
              />
            </Link>
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
  const [borrowingRequest, setBorrowingRequests] = useState<BorrowingRequest[]>(
    []
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

    setIsLoading(true);
    getBorrowingRequestsThisMonthService()
      .then((response) => {
        setBorrowingRequests(response.data);
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

  const handleBorrowBooks = () => {
    if (selectedBooks.length === 0) {
      message.warning('Please select at least one book to borrow');
      return;
    }

    if (selectedBooks.length > 5) {
      message.warning('Maximum 5 book can be requested at a time');
      return;
    }

    if (borrowingRequest.length >= 3) {
      message.warning('Maximum 3 borrowing requests per month');
      return;
    }

    setIsLoading(true);
    const requestDetails = selectedBooks.map((id) => {
      return {
        bookId: id,
      };
    });

    createBorrowingRequestService(requestDetails)
      .then(() => {
        message.success('Borrowing request created successfully');
        setSelectedBooks([]);
      })
      .catch((err) => {
        message.error(err.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Books List"
      createButton={
        <Button
          type="primary"
          icon={<ShoppingCartOutlined />}
          onClick={handleBorrowBooks}
        >
          Borrow Book
        </Button>
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
                <Checkbox
                  checked={selectedBooks.includes(book.id)}
                  onChange={(e) => {
                    if (e.target.checked) {
                      setSelectedBooks([...selectedBooks, book.id]);
                    } else {
                      setSelectedBooks(
                        selectedBooks.filter((id) => id !== book.id)
                      );
                    }
                  }}
                />,
                <Tooltip title="View details">
                  <Link
                    to={PATH.user.bookDetails.replace(
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

export default UserBookList;

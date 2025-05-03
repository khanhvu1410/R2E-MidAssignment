import { Button, message, Table, TableProps } from 'antd';
import { RequestDetails } from '../../models/requestDetails';
import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { getRequestDetailsByBorrowingRequestId } from '../../api/requestDetailsService';
import BodyLayout from '../layout/BodyLayout';
import { PATH } from '../../constants/paths';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { Book } from '../../models/book';

const RequestDetailsList = () => {
  const breadcrumbItems = [
    { title: <Link to={PATH.user.borrowingRequests}>Borrowing Request</Link> },
    { title: 'Details' },
  ];

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
      title: 'Quantity',
      dataIndex: 'quantity',
      key: 'quantity',
      align: 'center',
    },
  ];

  const { id } = useParams();
  const [books, setBooks] = useState<Book[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    getRequestDetailsByBorrowingRequestId(parseInt(id ?? '0'))
      .then((response) => {
        setBooks(
          response.data.map((requestDetails: RequestDetails) => {
            return {
              ...requestDetails.book,
              key: requestDetails.book.id,
            };
          })
        );
      })
      .catch((err) => {
        message.error(err.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Request Details"
      createButton={null}
    >
      {!isLoading && (
        <Table<Book>
          columns={columns}
          dataSource={books}
          scroll={{ x: 'max-content' }}
          size="middle"
          bordered
          pagination={false}
          style={{ marginBottom: 20 }}
        />
      )}
      <Link to={PATH.user.borrowingRequests}>
        <Button
          icon={<ArrowLeftOutlined />}
          type="text"
          style={{ marginRight: 10 }}
        >
          Back
        </Button>
      </Link>
    </BodyLayout>
  );
};

export default RequestDetailsList;

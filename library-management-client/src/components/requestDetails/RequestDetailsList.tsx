import { Button, Form, message, Select, Spin, Table, TableProps } from 'antd';
import { RequestDetails } from '../../models/requestDetails';
import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { getRequestDetailsByBorrowingRequestId } from '../../api/requestDetailsService';
import BodyLayout from '../layout/BodyLayout';
import { PATH } from '../../constants/paths';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { Book } from '../../models/book';
import { useAuthContext } from '../../context/AuthContext';
import { UserRole } from '../../models/auth';
import {
  getBorrowingRequestByIdService,
  updateBorrowingRequestService,
} from '../../api/borrowingRequestService';
import {
  BorrowingRequestToUpdate,
  RequestStatus,
} from '../../models/borrowingRequest';
import { FormProps, useForm } from 'antd/es/form/Form';

const RequestDetailsList = () => {
  const { user } = useAuthContext();
  const breadcrumbItems = [
    {
      title: (
        <Link
          to={
            user?.role === UserRole.SuperUser
              ? PATH.admin.borrowingRequests
              : PATH.user.borrowingRequests
          }
        >
          Borrowing Request
        </Link>
      ),
    },
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
  ];

  const { id } = useParams();
  const [books, setBooks] = useState<Book[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [form] = useForm();
  const navigate = useNavigate();

  useEffect(() => {
    setIsLoading(true);
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

    if (user?.role === UserRole.SuperUser) {
      setIsLoading(true);
      getBorrowingRequestByIdService(parseInt(id ?? '0'))
        .then((response) => {
          form.setFieldsValue({ status: response.data.status });
        })
        .catch((err) => {
          message.error(err.message);
        })
        .finally(() => {
          setIsLoading(false);
        });
    }
  }, [form, id, user?.role]);

  const onFinish: FormProps<BorrowingRequestToUpdate>['onFinish'] = (
    values
  ) => {
    setIsLoading(true);
    updateBorrowingRequestService(parseInt(id ?? '0'), values)
      .then(() => {
        navigate(PATH.admin.borrowingRequests);
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
      cardTitle="Request Details"
      createButton={null}
    >
      {isLoading ? (
        <div style={{ textAlign: 'center', padding: 24 }}>
          <Spin size="large" />
        </div>
      ) : (
        <div>
          <Table<Book>
            columns={columns}
            dataSource={books}
            scroll={{ x: 'max-content' }}
            size="middle"
            bordered
            pagination={false}
            style={{ marginBottom: 20 }}
          />

          {user?.role === UserRole.SuperUser && (
            <Form
              name="editBorrowingRequest"
              autoComplete="off"
              form={form}
              onFinish={onFinish}
              style={{
                marginBottom: 20,
                width: 200,
              }}
            >
              <Form.Item
                label="Status"
                name="status"
                rules={[
                  {
                    required: true,
                    message: 'Request status is required',
                  },
                ]}
              >
                <Select placeholder="Select a status">
                  <Select.Option
                    key={RequestStatus.Approved}
                    value={RequestStatus.Approved}
                  >
                    Approved
                  </Select.Option>
                  <Select.Option
                    key={RequestStatus.Rejected}
                    value={RequestStatus.Rejected}
                  >
                    Rejected
                  </Select.Option>
                  <Select.Option
                    key={RequestStatus.Waiting}
                    value={RequestStatus.Waiting}
                  >
                    Waiting
                  </Select.Option>
                </Select>
              </Form.Item>
              <Link
                to={PATH.admin.borrowingRequests}
                style={{ marginRight: 10 }}
              >
                <Button icon={<ArrowLeftOutlined />} type="text">
                  Back
                </Button>
              </Link>
              <Button type="primary" htmlType="submit">
                Submit
              </Button>
            </Form>
          )}
        </div>
      )}

      {user?.role === UserRole.NormalUser && (
        <Link to={PATH.user.borrowingRequests} style={{ marginRight: 10 }}>
          <Button icon={<ArrowLeftOutlined />} type="text">
            Back
          </Button>
        </Link>
      )}
    </BodyLayout>
  );
};

export default RequestDetailsList;

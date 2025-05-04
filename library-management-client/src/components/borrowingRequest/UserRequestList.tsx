import { DateTime } from 'luxon';
import {
  Button,
  message,
  Space,
  Spin,
  Table,
  TableProps,
  Tag,
  Tooltip,
} from 'antd';
import { BorrowingRequest } from '../../models/borrowingRequest';
import { Link } from 'react-router-dom';
import { EyeOutlined } from '@ant-design/icons';
import { useEffect, useState } from 'react';
import { PATH } from '../../constants/paths';
import { getBorrowingRequestsByRequestorId } from '../../api/borrowingRequestService';
import BodyLayout from '../layout/BodyLayout';

const defaultQueryParameters = {
  pageIndex: 1,
  pageSize: 5,
  totalCount: 0,
  search: '',
};

const UserRequestList = () => {
  const breadcrumbItems = [{ title: 'Borrowing Request' }];

  const columns: TableProps<BorrowingRequest>['columns'] = [
    {
      title: 'Requested Date',
      dataIndex: 'requestedDate',
      key: 'requestedDate',
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      align: 'center',
      render: (status: number) => {
        let statusText = '';
        let color = '';

        switch (status) {
          case 0:
            statusText = 'Approved';
            color = 'green';
            break;
          case 1:
            statusText = 'Rejected';
            color = 'red';
            break;
          case 2:
            statusText = 'Waiting';
            color = 'orange';
            break;
        }
        return <Tag color={color}>{statusText}</Tag>;
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
              to={PATH.user.requestDetails.replace(':id', record.id.toString())}
            >
              <Button
                type="text"
                icon={<EyeOutlined />}
                aria-label={`View ${record.requestedDate}`}
              />
            </Link>
          </Tooltip>
        </Space>
      ),
    },
  ];

  const [borrowingRequest, setBorrowingRequests] = useState<BorrowingRequest[]>(
    []
  );
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [borrowingRequestQueryParameters, setBorrowingRequestQueryParameters] =
    useState(defaultQueryParameters);

  useEffect(() => {
    let isSetData = true;
    getBorrowingRequestsByRequestorId(
      borrowingRequestQueryParameters.pageIndex,
      borrowingRequestQueryParameters.pageSize
    )
      .then((response) => {
        if (isSetData) {
          setBorrowingRequests(
            response.data.items.map((borrowingRequest: BorrowingRequest) => {
              return {
                ...borrowingRequest,
                requestedDate: DateTime.fromISO(borrowingRequest.requestedDate)
                  .toJSDate()
                  .toLocaleDateString('en-US'),
                key: borrowingRequest.id,
              };
            })
          );
          setBorrowingRequestQueryParameters((prev) => ({
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
  }, [
    borrowingRequestQueryParameters.pageIndex,
    borrowingRequestQueryParameters.pageSize,
  ]);

  const handleOnPageChange = (page: number, pageSize: number) => {
    setIsLoading(true);
    setBorrowingRequestQueryParameters({
      ...borrowingRequestQueryParameters,
      pageIndex: page,
      pageSize: pageSize,
    });
  };

  const handleOnPageSizeChange = (current: number, size: number) => {
    setIsLoading(true);
    setBorrowingRequestQueryParameters({
      ...borrowingRequestQueryParameters,
      pageIndex: current,
      pageSize: size,
    });
  };

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Borrowing Requests List"
      createButton={null}
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

      {!isLoading && (
        <Table<BorrowingRequest>
          columns={columns}
          dataSource={borrowingRequest}
          scroll={{ x: 'max-content' }}
          size="middle"
          bordered
          pagination={{
            pageSize: borrowingRequestQueryParameters.pageSize,
            current: borrowingRequestQueryParameters.pageIndex,
            total: borrowingRequestQueryParameters.totalCount,
            showSizeChanger: true,
            pageSizeOptions: [3, 5, 10],
            showTotal: (total) => `Total ${total} requests`,
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

export default UserRequestList;

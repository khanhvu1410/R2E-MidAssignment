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
  Tooltip,
} from 'antd';
import { Category } from '../../models/category';
import { Link } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { useMediaQuery } from 'react-responsive';
import { useEffect, useState } from 'react';
import {
  deleteCategoryService,
  getPagedCategoriesService,
} from '../../api/categoryService';
import BodyLayout from '../layout/BodyLayout';

const defaultQueryParameters = {
  pageIndex: 1,
  pageSize: 5,
  totalCount: 0,
  search: '',
};

const CategoryList = () => {
  const breadcrumbItems = [{ title: 'Category' }];

  const columns: TableProps<Category>['columns'] = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Actions',
      key: 'actions',
      align: 'center',
      width: 150,
      fixed: 'right',
      render: (_, record) => (
        <Space size="middle">
          <Tooltip title="Edit">
            <Link
              to={PATH.admin.editCategory.replace(':id', record.id.toString())}
            >
              <Button
                type="text"
                icon={<EditOutlined />}
                aria-label={`Edit ${record.name}`}
              />
            </Link>
          </Tooltip>
          <Tooltip title="Delete">
            <Popconfirm
              title="Delete this category?"
              onConfirm={() => {
                handleDeleteCategory(record.id);
              }}
            >
              <Button
                type="text"
                icon={<DeleteOutlined />}
                aria-label={`Delete ${record.name}`}
                danger
              />
            </Popconfirm>
          </Tooltip>
        </Space>
      ),
    },
  ];

  const isSmallDevice = useMediaQuery({ maxWidth: 768 });
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [categoryQueryParameters, setCategoryQueryParameters] = useState(
    defaultQueryParameters
  );

  useEffect(() => {
    let isSetData = true;
    getPagedCategoriesService(
      categoryQueryParameters.pageIndex,
      categoryQueryParameters.pageSize
    )
      .then((response) => {
        if (isSetData) {
          setCategories(
            response.data.items.map((category: Category) => {
              return {
                ...category,
                key: category.id,
              };
            })
          );
          setCategoryQueryParameters((prev) => ({
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
  }, [categoryQueryParameters.pageIndex, categoryQueryParameters.pageSize]);

  const handleDeleteCategory = (id: number) => {
    deleteCategoryService(id)
      .then(() => {
        // Filter out the deleted category
        setCategories((prevCategories) =>
          prevCategories.filter((category) => category.id !== id)
        );

        // Update total count
        setCategoryQueryParameters((prev) => ({
          ...prev,
          totalCount: prev.totalCount - 1,
        }));

        // If the current page would be empty after deletion, go to previous page
        if (categories.length === 1 && categoryQueryParameters.pageIndex > 1) {
          setCategoryQueryParameters((prev) => ({
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
    setCategoryQueryParameters({
      ...categoryQueryParameters,
      pageIndex: page,
      pageSize: pageSize,
    });
  };

  const handleOnPageSizeChange = (current: number, size: number) => {
    setIsLoading(true);
    setCategoryQueryParameters({
      ...categoryQueryParameters,
      pageIndex: current,
      pageSize: size,
    });
  };

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Categories List"
      createButton={
        <Link to={PATH.admin.createCategory}>
          <Button type="primary" icon={<PlusOutlined />}>
            Create Category
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

        {isSmallDevice && !isLoading && (
          <List
            dataSource={categories}
            pagination={{
              pageSize: categoryQueryParameters.pageSize,
              current: categoryQueryParameters.pageIndex,
              total: categoryQueryParameters.totalCount,
              showSizeChanger: true,
              pageSizeOptions: [3, 5, 10],
              showTotal: (total) => `Total ${total} categories`,
              onChange: (page, pageSize) => {
                handleOnPageChange(page, pageSize);
              },
              onShowSizeChange: (current, size) => {
                handleOnPageSizeChange(current, size);
              },
            }}
            renderItem={(category) => (
              <Card
                title={category.name}
                size="small"
                style={{ marginBottom: 16 }}
                actions={[
                  <Tooltip title="Edit">
                    <Link
                      to={PATH.admin.editCategory.replace(
                        ':id',
                        category.id.toString()
                      )}
                    >
                      <Button
                        type="text"
                        icon={<EditOutlined />}
                        onClick={() => category}
                        aria-label={`Edit ${category.name}`}
                      />
                    </Link>
                  </Tooltip>,
                  <Tooltip title="Delete">
                    <Popconfirm
                      title="Are you sure to delete this book?"
                      onConfirm={() => handleDeleteCategory(category.id)}
                      okText="Yes"
                      cancelText="No"
                    >
                      <Button
                        danger
                        type="text"
                        icon={<DeleteOutlined />}
                        aria-label={`Delete ${category.name}`}
                      />
                    </Popconfirm>
                  </Tooltip>,
                ]}
              >
                <p>
                  <strong>Name:</strong> {category.name}
                </p>
              </Card>
            )}
          />
        )}

        {!isLoading && !isSmallDevice && (
          <Table<Category>
            columns={columns}
            dataSource={categories}
            scroll={{ x: 'max-content' }}
            size="middle"
            bordered
            pagination={{
              pageSize: categoryQueryParameters.pageSize,
              current: categoryQueryParameters.pageIndex,
              total: categoryQueryParameters.totalCount,
              showSizeChanger: true,
              pageSizeOptions: [3, 5, 10],
              showTotal: (total) => `Total ${total} categories`,
              onChange: (page, pageSize) => {
                handleOnPageChange(page, pageSize);
              },
              onShowSizeChange: (current, size) => {
                handleOnPageSizeChange(current, size);
              },
            }}
          />
        )}
      </div>
    </BodyLayout>
  );
};

export default CategoryList;

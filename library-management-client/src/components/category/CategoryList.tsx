import {
  Button,
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
      width: 125,
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
    </BodyLayout>
  );
};

export default CategoryList;

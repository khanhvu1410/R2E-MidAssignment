import { ArrowLeftOutlined } from '@ant-design/icons';
import {
  Button,
  Col,
  Form,
  FormProps,
  Input,
  InputNumber,
  message,
  Row,
  Select,
  Spin,
} from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import BodyLayout from '../layout/BodyLayout';
import { Book } from '../../models/book';
import { PATH } from '../../constants/paths';
import { createBookService } from '../../api/bookService';
import { useEffect, useState } from 'react';
import { Category } from '../../models/category';
import { getAllCategoriesService } from '../../api/categoryService';

const CreateBook = () => {
  const breadcrumbItems = [
    { title: <Link to={PATH.admin.books}>Book</Link> },
    { title: 'Create' },
  ];

  const currentYear = new Date().getFullYear();
  const navigate = useNavigate();
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    getAllCategoriesService()
      .then((response) => {
        setCategories(response.data);
      })
      .catch((err) => {
        message.error(err.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  const onFinish: FormProps<Book>['onFinish'] = (values) => {
    createBookService(values)
      .then(() => {
        navigate(PATH.admin.books);
      })
      .catch((error) => {
        message.error(error.message);
      });
  };

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Create Book"
      createButton={null}
    >
      <Row justify="center">
        <Col xs={24} sm={20} md={16} lg={12}>
          {isLoading ? (
            <div style={{ textAlign: 'center', padding: 24 }}>
              <Spin size="large" />
            </div>
          ) : (
            <Form<Book>
              name="createBookForm"
              layout="vertical"
              autoComplete="off"
              onFinish={onFinish}
            >
              <Form.Item
                label="Title"
                name="title"
                rules={[
                  { required: true, message: 'Please input the book title' },
                  { max: 200, message: 'Title cannot exceed 200 characters' },
                  {
                    whitespace: true,
                    message: 'Title cannot be whitespace',
                  },
                ]}
              >
                <Input placeholder="Enter book title" />
              </Form.Item>

              <Form.Item
                label="Author"
                name="author"
                rules={[
                  { required: true, message: 'Please input the author name' },
                  {
                    max: 100,
                    message: 'Author name cannot exceed 100 characters',
                  },
                ]}
              >
                <Input placeholder="Enter author name" />
              </Form.Item>

              <Form.Item
                label="ISBN"
                name="isbn"
                rules={[
                  { required: true, message: 'Please input the ISBN' },
                  {
                    min: 10,
                    max: 13,
                    message: 'ISBN must be 10 or 13 characters',
                  },
                  {
                    pattern: /^[0-9]+$/,
                    message: 'ISBN contains invalid characters',
                  },
                ]}
              >
                <Input placeholder="Enter ISBN" />
              </Form.Item>

              <Form.Item
                label="Pulication Year"
                name="publicationYear"
                rules={[
                  { required: true, message: 'Please input the ISBN' },
                  {
                    type: 'number',
                    min: 1800,
                    max: currentYear,
                    message: `Publication year must be between 1800 and ${currentYear}`,
                  },
                ]}
              >
                <InputNumber
                  style={{ width: '100%' }}
                  placeholder="Enter ISBN"
                />
              </Form.Item>

              <Form.Item
                label="Description"
                name="description"
                rules={[
                  {
                    required: true,
                    message: 'Please input the book description',
                  },
                ]}
              >
                <Input.TextArea rows={4} placeholder="Enter book description" />
              </Form.Item>

              <Form.Item
                label="Quantity"
                name="quantity"
                rules={[
                  { required: true, message: 'Quantity is required' },
                  {
                    type: 'number',
                    min: 0,
                    message: 'Quantity must be 0 or more',
                  },
                ]}
              >
                <InputNumber
                  style={{ width: '100%' }}
                  placeholder="Enter quantity"
                  min={0}
                />
              </Form.Item>

              <Form.Item
                label="Category"
                name="categoryId"
                rules={[
                  {
                    required: true,
                    message: 'Category is required',
                  },
                ]}
              >
                <Select placeholder="Select a category">
                  {categories.map((category) => (
                    <Select.Option key={category.id} value={category.id}>
                      {category.name}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>

              <Form.Item>
                <Link to={PATH.admin.books}>
                  <Button
                    icon={<ArrowLeftOutlined />}
                    type="text"
                    style={{ marginRight: 10 }}
                  >
                    Back
                  </Button>
                </Link>

                <Button type="primary" htmlType="submit">
                  Submit
                </Button>
              </Form.Item>
            </Form>
          )}
        </Col>
      </Row>
    </BodyLayout>
  );
};

export default CreateBook;

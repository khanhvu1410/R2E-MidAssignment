import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { Button, Col, Form, FormProps, Input, message, Row, Spin } from 'antd';
import { Category } from '../../models/category';
import {
  getCategoryByIdService,
  updateCategoryService,
} from '../../api/categoryService';
import BodyLayout from '../layout/BodyLayout';
import { ArrowLeftOutlined } from '@ant-design/icons';

const EditCategory = () => {
  const [title, setTitle] = useState<string>('');
  const breadcrumbItems = [
    { title: <Link to={PATH.admin.categories}>Category</Link> },
    { title: `${title}` },
    { title: 'Create' },
  ];

  const navigate = useNavigate();
  const { id } = useParams();
  const [form] = Form.useForm();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    getCategoryByIdService(parseInt(id ?? '0'))
      .then((response) => {
        form.setFieldsValue(response.data);
        setTitle(response.data.name);
      })
      .catch((error) => {
        message.error(error.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [form, id]);

  const onFinish: FormProps<Category>['onFinish'] = (values) => {
    setIsLoading(true);
    updateCategoryService(values, parseInt(id ?? '0'))
      .then(() => {
        navigate(PATH.admin.categories);
      })
      .catch((error) => {
        message.error(error.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Create Category"
      createButton={null}
    >
      <Row justify="center">
        <Col xs={24} sm={20} md={16} lg={12}>
          {isLoading ? (
            <div style={{ textAlign: 'center', padding: 24 }}>
              <Spin size="large" />
            </div>
          ) : (
            <Form<Category>
              name="createCategoryForm"
              layout="vertical"
              autoComplete="off"
              form={form}
              onFinish={onFinish}
            >
              <Form.Item
                label="Name"
                name="name"
                rules={[
                  { required: true, message: 'Please input the category name' },
                  {
                    whitespace: true,
                    message: 'Name cannot be whitespace',
                  },
                ]}
              >
                <Input placeholder="Enter category name" />
              </Form.Item>

              <Form.Item>
                <Link to={PATH.admin.categories}>
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

export default EditCategory;

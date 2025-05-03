import { Link, useNavigate } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { Category } from '../../models/category';
import { Button, Col, Form, FormProps, Input, message, Row } from 'antd';
import { createCategoryService } from '../../api/categoryService';
import BodyLayout from '../layout/BodyLayout';
import { ArrowLeftOutlined } from '@ant-design/icons';

const CreateCategory = () => {
  const breadcrumbItems = [
    { title: <Link to={PATH.admin.categories}>Category</Link> },
    { title: 'Create' },
  ];

  const navigate = useNavigate();

  const onFinish: FormProps<Category>['onFinish'] = (values) => {
    createCategoryService(values)
      .then(() => {
        navigate(PATH.admin.categories);
      })
      .catch((error) => {
        message.error(error.message);
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
          <Form<Category>
            name="createCategoryForm"
            layout="vertical"
            autoComplete="off"
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
        </Col>
      </Row>
    </BodyLayout>
  );
};

export default CreateCategory;

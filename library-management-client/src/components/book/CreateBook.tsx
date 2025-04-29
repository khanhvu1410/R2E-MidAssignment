import { ArrowLeftOutlined } from '@ant-design/icons';
import {
  Button,
  Col,
  DatePicker,
  Form,
  FormProps,
  Input,
  Row,
  Select,
} from 'antd';
import BodyLayout from '../layout/BodyLayout';
import { Book } from '../../models/book';
import { Link, useNavigate } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { createBookService } from '../../api/bookService';

const CreateBook = () => {
  const breadcrumbItems = [
    { title: <Link to={PATH.books}>Book</Link> },
    { title: 'Create' },
  ];

  const navigate = useNavigate();

  const onFinish: FormProps<Book>['onFinish'] = (values) => {
    createBookService(values)
      .then(() => {
        navigate(PATH.books);
      })
      .catch((error) => {
        alert(error.message);
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
              ]}
            >
              <Input placeholder="Enter book title" />
            </Form.Item>

            <Form.Item
              label="Author"
              name="author"
              rules={[
                { required: true, message: 'Please input the author name' },
              ]}
            >
              <Input placeholder="Enter author name" />
            </Form.Item>

            <Form.Item label="Published Date" name="publishedDate">
              <DatePicker style={{ width: '100%' }} />
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
              label="Category"
              name="category"
              rules={[
                {
                  required: true,
                  message: 'Please input the category name',
                },
              ]}
            >
              <Input placeholder="Enter category name" />
            </Form.Item>

            <Form.Item label="Status" name="status" initialValue="available">
              <Select>
                <Select.Option value="available">Available</Select.Option>
                <Select.Option value="checked">Checked out</Select.Option>
              </Select>
            </Form.Item>

            <Form.Item>
              <Link to={PATH.books}>
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

export default CreateBook;

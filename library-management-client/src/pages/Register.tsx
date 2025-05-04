import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { LockOutlined, MailOutlined, UserOutlined } from '@ant-design/icons';
import { Button, Card, Divider, Form, Input, message } from 'antd';
import { registerService } from '../api/authService';
import { PATH } from '../constants/paths';

interface RegisterFormValues {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

const Register = () => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const onFinish = (values: RegisterFormValues) => {
    if (values.confirmPassword !== values.password) {
      message.error('Passwords do not match');
      return;
    }
    const registerData = {
      username: values.username,
      email: values.email,
      password: values.password,
    };
    setIsLoading(true);
    registerService(registerData)
      .then(() => {
        message.success('Registration successfull!');
        navigate(PATH.auth.login);
      })
      .catch((err) => {
        message.error(err.message);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <div
      style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100vh',
      }}
    >
      <Card title="Register" style={{ width: 400 }}>
        <Form name="register" onFinish={onFinish}>
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please input your username!' }]}
          >
            <Input prefix={<UserOutlined />} placeholder="Username" />
          </Form.Item>
          <Form.Item
            name="email"
            rules={[
              { required: true, message: 'Please input your email!' },
              { type: 'email', message: 'Please enter a valid email!' },
            ]}
          >
            <Input prefix={<MailOutlined />} placeholder="Email" />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please input your password!' }]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="Password" />
          </Form.Item>
          <Form.Item
            name="confirmPassword"
            rules={[
              { required: true, message: 'Please confirm your password!' },
            ]}
          >
            <Input.Password
              prefix={<LockOutlined />}
              placeholder="Confirm Password"
            />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" loading={isLoading} block>
              Register
            </Button>
          </Form.Item>
        </Form>

        <Divider />

        <Link
          to={PATH.auth.login}
          style={{ display: 'flex', justifyContent: 'center' }}
        >
          <Button type="link">Already have an account? Login</Button>
        </Link>
      </Card>
    </div>
  );
};

export default Register;

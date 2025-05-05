import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuthContext } from '../context/AuthContext';
import { LoginCredentials, UserRole } from '../models/auth';
import { loginService } from '../api/authService';
import { Button, Divider, Form, Input, message } from 'antd';
import Card from 'antd/es/card/Card';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { PATH } from '../constants/paths';

const Login = () => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { login } = useAuthContext();

  const onFinish = (values: LoginCredentials) => {
    setIsLoading(true);
    loginService(values)
      .then((response) => {
        const { user, accessToken } = response.data;
        login(user, accessToken);
        if (user?.role === UserRole.SuperUser) {
          navigate(PATH.admin.dashboard);
        } else if (user?.role === UserRole.NormalUser) {
          navigate(PATH.user.books);
        }
      })
      .catch((err) => {
        message.error('Login failed! The username or password is incorrect.');
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
      <Card title="Login" style={{ width: 400 }}>
        <Form name="login" onFinish={onFinish}>
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please input your username!' }]}
          >
            <Input prefix={<UserOutlined />} placeholder="Username" />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please input your password!' }]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="Password" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" loading={isLoading} block>
              Login
            </Button>
          </Form.Item>
        </Form>

        <Divider />

        <Link
          to={PATH.auth.register}
          style={{ display: 'flex', justifyContent: 'center' }}
        >
          <Button type="link">Need an account? Register</Button>
        </Link>
      </Card>
    </div>
  );
};

export default Login;

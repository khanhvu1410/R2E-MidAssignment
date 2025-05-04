import { Avatar, Dropdown, MenuProps } from 'antd';
import { Header } from 'antd/es/layout/layout';
import { useAuthContext } from '../../context/AuthContext';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { UserRole } from '../../models/auth';

const CustomHeader = () => {
  const { user, logout } = useAuthContext();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate(PATH.auth.login);
  };

  const menuItems: MenuProps['items'] = [
    {
      key: 'username',
      label: `Username: ${user?.username}`,
      disabled: true,
    },
    {
      key: 'email',
      label: `Email: ${user?.email}`,
      disabled: true,
    },
    {
      key: 'role',
      label: `User Role: ${
        user?.role === UserRole.SuperUser ? 'Super' : 'Normal'
      }`,
      disabled: true,
    },
    {
      type: 'divider',
    },
    {
      key: 'logout',
      label: 'Logout',
      icon: <LogoutOutlined />,
      onClick: handleLogout,
    },
  ];

  return (
    <Header
      style={{
        display: 'flex',
        flexDirection: 'row',
        padding: '0 20px',
        justifyContent: 'space-between',
        alignItems: 'center',
        backgroundColor: '#4096ff',
      }}
    >
      <div style={{ fontWeight: '500', fontSize: 20, color: '#fff' }}>
        Library Management System
      </div>

      <Dropdown menu={{ items: menuItems }} placement="bottomRight" arrow>
        <Avatar
          style={{
            backgroundColor: '#fff',
            color: '#4096ff',
            cursor: 'pointer',
          }}
          icon={<UserOutlined />}
        />
      </Dropdown>
    </Header>
  );
};

export default CustomHeader;

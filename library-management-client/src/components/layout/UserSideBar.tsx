import { BookOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import { Menu, MenuProps } from 'antd';
import Sider from 'antd/es/layout/Sider';
import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { PATH } from '../../constants/paths';

const UserSideBar = () => {
  const menuItems: MenuProps['items'] = [
    {
      key: PATH.user.books,
      label: 'Borrowing Books',
      icon: BookOutlined,
    },
    {
      key: PATH.user.borrowingRequests,
      label: 'My Borrowings',
      icon: ShoppingCartOutlined,
    },
  ].map((item) => {
    return {
      key: item.key,
      label: item.label,
      icon: React.createElement(item.icon),
    };
  });

  const location = useLocation();
  const navigate = useNavigate();

  return (
    <Sider width={200} breakpoint="lg" collapsedWidth={0}>
      <Menu
        mode="inline"
        items={menuItems}
        style={{ height: '100%', borderRight: 0 }}
        onClick={({ key }) => {
          navigate(key);
        }}
        selectedKeys={[location.pathname]}
      />
    </Sider>
  );
};

export default UserSideBar;

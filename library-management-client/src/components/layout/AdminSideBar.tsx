import {
  AppstoreOutlined,
  BookOutlined,
  ShoppingCartOutlined,
  TagOutlined,
} from '@ant-design/icons';
import { Menu, MenuProps } from 'antd';
import Sider from 'antd/es/layout/Sider';
import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { PATH } from '../../constants/paths';

const AdminSideBar = () => {
  const menuItems: MenuProps['items'] = [
    {
      key: PATH.admin.dashboard,
      label: 'Dashboard',
      icon: AppstoreOutlined,
    },
    {
      key: PATH.admin.books,
      label: 'Books',
      icon: BookOutlined,
    },
    {
      key: PATH.admin.categories,
      label: 'Categories',
      icon: TagOutlined,
    },
    {
      key: PATH.admin.borrowingRequests,
      label: 'Borrowing Requests',
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

export default AdminSideBar;

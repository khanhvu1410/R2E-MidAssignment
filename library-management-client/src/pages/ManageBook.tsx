import { Layout } from 'antd';
import CustomHeader from '../components/layout/CustomHeader';
import AdminSideBar from '../components/layout/AdminSideBar';
import { Outlet } from 'react-router-dom';

const ManageBook = () => {
  return (
    <Layout style={{ minHeight: '100vh' }}>
      <CustomHeader />
      <Layout>
        <AdminSideBar />
        <Outlet />
      </Layout>
    </Layout>
  );
};

export default ManageBook;

import { Alert, Layout } from 'antd';
import CustomHeader from '../../components/layout/CustomHeader';
import SideBar from '../../components/layout/SideBar';
import { Outlet } from 'react-router-dom';

const ManageBook = () => {
  return (
    <Layout style={{ minHeight: '100vh' }}>
      <CustomHeader />
      <Layout>
        <SideBar />
        <Outlet />
      </Layout>
    </Layout>
  );
};

export default ManageBook;

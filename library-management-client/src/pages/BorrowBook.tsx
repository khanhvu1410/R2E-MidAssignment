import { Layout } from 'antd';
import CustomHeader from '../components/layout/CustomHeader';
import UserSideBar from '../components/layout/UserSideBar';
import { Outlet } from 'react-router-dom';

const BorrowBook = () => {
  return (
    <Layout style={{ minHeight: '100vh' }}>
      <CustomHeader />
      <Layout>
        <UserSideBar />
        <Outlet />
      </Layout>
    </Layout>
  );
};

export default BorrowBook;

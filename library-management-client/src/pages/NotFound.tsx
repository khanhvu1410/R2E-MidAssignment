import { Empty } from 'antd';

const NotFound = () => {
  return (
    <div
      style={{
        height: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <Empty description="Page not found" />
    </div>
  );
};

export default NotFound;

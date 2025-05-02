import { Header } from 'antd/es/layout/layout';

const CustomHeader = () => {
  return (
    <Header
      style={{
        display: 'flex',
        flexDirection: 'row',
        padding: '0 20px',
        justifyContent: 'space-between',
        backgroundColor: '#4096ff',
      }}
    >
      <div style={{ fontWeight: '500', fontSize: 20, color: '#fff' }}>
        Library Management System
      </div>
    </Header>
  );
};

export default CustomHeader;

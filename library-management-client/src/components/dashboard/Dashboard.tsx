import BodyLayout from '../layout/BodyLayout';

const Dashboard = () => {
  const breadcrumbItems = [
    {
      title: 'Dashboard',
    },
  ];

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle="Dashboard"
      createButton={null}
    >
      <div>Dashboard</div>
    </BodyLayout>
  );
};

export default Dashboard;

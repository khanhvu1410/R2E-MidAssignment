import { Breadcrumb, Card, Layout } from 'antd';
import { ReactNode } from 'react';

interface BodyLayoutProps {
  children: ReactNode;
  breadcrumbItems: { title: string | ReactNode }[];
  cardTitle: string;
  createButton: ReactNode | null;
}

const BodyLayout = (props: BodyLayoutProps) => {
  return (
    <Layout style={{ padding: '0 24px 24px' }}>
      <Breadcrumb items={props.breadcrumbItems} style={{ margin: '16px 0' }} />
      <Card
        title={props.cardTitle}
        extra={props.createButton}
        style={{ height: '100%' }}
      >
        {props.children}
      </Card>
    </Layout>
  );
};

export default BodyLayout;

import { Button, Descriptions, message } from 'antd';
import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { getBookByIdService } from '../../api/bookService';
import { Book } from '../../models/book';
import BodyLayout from '../layout/BodyLayout';
import { ArrowLeftOutlined } from '@ant-design/icons';

const BookDetails = () => {
  const [title, setTitle] = useState<string>('');
  const breadcrumbItems = [
    { title: <Link to={PATH.admin.books}>Book</Link> },
    { title: `${title}` },
    { title: 'Details' },
  ];

  const { id } = useParams();
  const [book, setBook] = useState<Book>({
    id: 0,
    title: '',
    author: '',
    isbn: '',
    publicationYear: 0,
    description: '',
    quantity: 0,
    categoryId: 0,
    categoryName: '',
  });

  useEffect(() => {
    getBookByIdService(parseInt(id ?? '0'))
      .then((response) => {
        setTitle(response.data.title);
        setBook(response.data);
      })
      .catch((error) => {
        message.error(error.message);
      });
  }, [id]);

  return (
    <BodyLayout
      breadcrumbItems={breadcrumbItems}
      cardTitle={title}
      createButton={null}
    >
      <Descriptions bordered column={1} style={{ marginBottom: 20 }}>
        <Descriptions.Item label="Title">{book.title}</Descriptions.Item>
        <Descriptions.Item label="Author">{book.author}</Descriptions.Item>
        <Descriptions.Item label="ISBN">{book.isbn}</Descriptions.Item>
        <Descriptions.Item label="Publication year">
          {book.publicationYear}
        </Descriptions.Item>
        <Descriptions.Item label="Description">
          {book.description}
        </Descriptions.Item>
        <Descriptions.Item label="Quantity">{book.quantity}</Descriptions.Item>
        <Descriptions.Item label="Category name">
          {book.categoryName}
        </Descriptions.Item>
      </Descriptions>
      <Link to={PATH.admin.books}>
        <Button
          icon={<ArrowLeftOutlined />}
          type="text"
          style={{ marginRight: 10 }}
        >
          Back
        </Button>
      </Link>
    </BodyLayout>
  );
};

export default BookDetails;

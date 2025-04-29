import { PATH } from '../constants/paths';
import CreateBook from '../components/book/CreateBook';
import BookList from '../components/book/BookList';
import Dashboard from '../components/dashboard/Dashboard';
import ManageBook from '../pages/admin/ManageBook';
import EditBook from '../components/book/EditBook';

export const AppRoutes = [
  {
    element: <ManageBook />,
    children: [
      { path: PATH.dashboard, element: <Dashboard /> },
      { path: PATH.books, element: <BookList /> },
      { path: PATH.createBook, element: <CreateBook /> },
      { path: PATH.editBook, element: <EditBook /> },
    ],
  },
];

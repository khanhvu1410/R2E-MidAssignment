import { PATH } from '../constants/paths';
import CreateBook from '../components/book/CreateBook';
import BookList from '../components/book/BookList';
import Dashboard from '../components/dashboard/Dashboard';
import ManageBook from '../pages/admin/ManageBook';
import EditBook from '../components/book/EditBook';
import Login from '../pages/auth/Login';
import PrivateRoute from '../components/auth/PrivateRoute';
import { UserRole } from '../models/auth';

export const AppRoutes = [
  {
    path: PATH.auth.login,
    element: <Login />,
  },
  {
    element: (
      <PrivateRoute requiredRole={UserRole.SuperUser}>
        <ManageBook />
      </PrivateRoute>
    ),
    children: [
      { path: PATH.admin.dashboard, element: <Dashboard /> },
      { path: PATH.admin.books, element: <BookList /> },
      { path: PATH.admin.createBook, element: <CreateBook /> },
      { path: PATH.admin.editBook, element: <EditBook /> },
    ],
  },
];

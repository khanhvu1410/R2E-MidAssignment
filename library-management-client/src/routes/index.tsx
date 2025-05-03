import { PATH } from '../constants/paths';
import { UserRole } from '../models/auth';
import CreateBook from '../components/book/CreateBook';
import AdminBookList from '../components/book/AdminBookList';
import Dashboard from '../components/dashboard/Dashboard';
import ManageBook from '../pages/ManageBook';
import EditBook from '../components/book/EditBook';
import Login from '../pages/Login';
import PrivateRoute from '../components/auth/PrivateRoute';
import BookDetails from '../components/book/BookDetails';
import CategoryList from '../components/category/CategoryList';
import CreateCategory from '../components/category/CreateCategory';
import EditCategory from '../components/category/EditCategory';
import BorrowBook from '../pages/BorrowBook';
import UserBookList from '../components/book/UserBookList';
import UserBorrowingRequests from '../components/borrowingRequest/UserBorrowingRequests';
import RequestDetailsList from '../components/requestDetails/RequestDetailsList';
import NotFound from '../pages/NotFound';

export const AppRoutes = [
  {
    path: '*',
    element: <NotFound />,
  },
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
      { path: PATH.admin.books, element: <AdminBookList /> },
      { path: PATH.admin.bookDetails, element: <BookDetails /> },
      { path: PATH.admin.createBook, element: <CreateBook /> },
      { path: PATH.admin.editBook, element: <EditBook /> },
      { path: PATH.admin.categories, element: <CategoryList /> },
      { path: PATH.admin.createCategory, element: <CreateCategory /> },
      { path: PATH.admin.editCategory, element: <EditCategory /> },
    ],
  },
  {
    element: (
      <PrivateRoute requiredRole={UserRole.NormalUser}>
        <BorrowBook />
      </PrivateRoute>
    ),
    children: [
      { path: PATH.user.books, element: <UserBookList /> },
      { path: PATH.user.bookDetails, element: <BookDetails /> },
      { path: PATH.user.borrowingRequests, element: <UserBorrowingRequests /> },
      { path: PATH.user.requestDetails, element: <RequestDetailsList /> },
    ],
  },
];

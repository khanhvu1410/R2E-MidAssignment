export const PATH = {
  auth: {
    login: '/',
    register: '/register',
  },
  admin: {
    dashboard: '/admin/dashboard',
    books: '/admin/books',
    bookDetails: '/admin/books/:id',
    createBook: '/admin/books/create',
    editBook: '/admin/books/:id/edit',
    categories: '/admin/categories',
    createCategory: '/admin/categories/create',
    editCategory: '/admin/categories/:id/edit',
    borrowingRequests: '/admin/borrowingRequests',
    requestDetails: '/admin/borrowingRequests/:id/details',
  },
  user: {
    books: '/books',
    bookDetails: '/books/:id',
    createBorrowingRequest: '/borrowingRequests/create',
    borrowingRequests: '/borrowingRequests/',
    requestDetails: '/borrowingRequests/:id/details',
  },
};

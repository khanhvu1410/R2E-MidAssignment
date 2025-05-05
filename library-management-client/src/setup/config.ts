export const ROOT_API = {
  baseURL: 'https://localhost:7295/',
};

export const ENDPOINT_API = {
  auth: {
    login: '/auth/login',
    register: '/auth/register',
  },
  book: {
    getPaged: '/books?pageIndex=:pageIndex&pageSize=:pageSize',
    create: '/books',
    getById: '/books/:id',
    delete: '/books/:id',
    update: '/books/:id',
  },
  category: {
    getPaged: '/categories?pageIndex=:pageIndex&pageSize=:pageSize',
    getAll: '/categories/all',
    create: '/categories',
    getById: '/categories/:id',
    delete: '/categories/:id',
    update: '/categories/:id',
  },
  borrowingRequest: {
    getPaged: '/borrowingRequests?pageIndex=:pageIndex&pageSize=:pageSize',
    create: '/borrowingRequests',
    getThisMonth: '/borrowingRequests/thisMonth',
    getByRequestorId:
      '/borrowingRequests/getByRequestorId?pageIndex=:pageIndex&pageSize=:pageSize',
    getById: '/borrowingRequests/:id',
    update: '/borrowingRequests/:id',
  },
  requestDetails: {
    getByBorrowingRequestId: '/requestDetails/:borrowingRequestId',
  },
};

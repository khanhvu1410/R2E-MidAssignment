export const ROOT_API = {
  baseURL: 'https://localhost:7295/',
};

export const ENDPOINT_API = {
  auth: {
    login: '/auth/login',
    register: '/auth/register',
  },
  book: {
    getAll: '/books?pageIndex=:pageIndex&pageSize=:pageSize',
    create: '/books',
    getById: '/books/:id',
    delete: '/books/:id',
    update: '/books/:id',
  },
};

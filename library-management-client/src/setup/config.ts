export const ROOT_API = {
  baseURL: 'http://localhost:8000',
};

export const ENDPOINT_API = {
  book: {
    getAll: '/books',
    create: '/books',
    getById: '/books/:id',
    delete: '/books/:id',
    update: '/books/:id',
  },
};

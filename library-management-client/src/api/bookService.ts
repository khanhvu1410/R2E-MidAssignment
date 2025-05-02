import { Book } from '../models/book';
import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const getAllBooksService = (pageIndex: number, pageSize: number) => {
  return httpClient.get(
    ENDPOINT_API.book.getAll
      .replace(':pageIndex', pageIndex.toString())
      .replace(':pageSize', pageSize.toString())
  );
};

export const createBookService = (book: Book) => {
  return httpClient.post(ENDPOINT_API.book.create, book);
};

export const deleteBookService = (id: number) => {
  return httpClient.delete(
    ENDPOINT_API.book.delete.replace(':id', id.toString())
  );
};

export const getBookByIdService = (id: number) => {
  return httpClient.get(
    ENDPOINT_API.book.getById.replace(':id', id.toString())
  );
};

export const updateBookService = (book: Book, id: number) => {
  return httpClient.put(
    ENDPOINT_API.book.update.replace(':id', id.toString()),
    book
  );
};

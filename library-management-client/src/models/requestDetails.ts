import { Book } from './book';

export interface RequestDetailsToAdd {
  bookId: number;
}

export interface RequestDetails {
  book: Book;
}

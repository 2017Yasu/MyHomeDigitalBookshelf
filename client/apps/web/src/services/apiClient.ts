import axios from 'axios';

const apiClient = axios.create({
  baseURL: '/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const auth = {
  register: async (username: string, email: string, password: string) => {
    const response = await apiClient.post('/auth/register', { username, email, password });
    return response.data;
  },

  login: async (email: string, password: string) => {
    const response = await apiClient.post('/auth/login', { email, password });
    return response.data; // Should contain the JWT token
  },
};

export const books = {
  addBookFromIsbn: async (isbn: string, bookshelfId: string, ownerId: string) => {
    const response = await apiClient.post('/books/from-isbn', { isbn, bookshelfId, ownerId });
    return response.data;
  },
  getBooksForBookshelf: async (bookshelfId: string, params?: object) => {
    const response = await apiClient.get(`/bookshelves/${bookshelfId}/books`, { params });
    return response.data;
  },
  getBookById: async (id: string) => {
    const response = await apiClient.get(`/books/${id}`);
    return response.data;
  },
  updateUserBookStatus: async (bookId: string, userId: string, newReadingStatus?: string, newLoanStatus?: string) => {
    const response = await apiClient.put(`/user-books/${bookId}/status`, { userId, newReadingStatus, newLoanStatus });
    return response.data;
  },
  addBookManually: async (bookData: {
    title: string;
    authors: string[];
    isbn?: string;
    publisher?: string;
    publishDate?: string;
    bookshelfId: string;
    ownerId: string;
  }) => {
    const response = await apiClient.post('/books', bookData);
    return response.data;
  },
};

export const bookshelves = {
  createBookshelf: async (name: string, description: string, ownerId: string) => {
    const response = await apiClient.post('/bookshelves', { name, description, ownerId });
    return response.data;
  },
  getBookshelfById: async (id: string) => {
    const response = await apiClient.get(`/bookshelves/${id}`);
    return response.data;
  },
};

export default apiClient;

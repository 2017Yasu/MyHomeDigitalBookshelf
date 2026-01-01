import axios from 'axios';
import Constants from 'expo-constants';

// For Expo, API_BASE_URL can be configured in app.config.js or via environment variables
// This is a placeholder and might need adjustment based on how the Expo app is configured.
// For development, you might use your local machine's IP address.
const API_BASE_URL = Constants.manifest?.extra?.API_BASE_URL || 'http://localhost:5000/api/v1';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
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
  getBooksForBookshelf: async (bookshelfId: string, params?: any) => {
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
  addBookManually: async (bookData: { title: string; authors: string[]; isbn?: string; publisher?: string; publishDate?: string; bookshelfId: string; ownerId: string }) => {
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

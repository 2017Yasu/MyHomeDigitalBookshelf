import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api/v1'; // Default to localhost

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

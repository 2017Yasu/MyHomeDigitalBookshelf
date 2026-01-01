import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { books } from '../services/apiClient';
import { useAuth } from '../context/useAuth';
import { type AxiosError } from 'axios'; // Added

interface Book {
  id: string;
  title: string;
  authors: string[];
  isbn: string;
  // Add other book properties as needed
}

const BookDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth(); // Removed token
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }

    const fetchBookDetails = async () => {
      try {
        setLoading(true);
        setError('');
        if (id) {
          const fetchedBook = await books.getBookById(id);
          setBook(fetchedBook);
        }
      } catch (err: unknown) {
        const axiosError = err as AxiosError<{ message: string }>;
        setError(axiosError.response?.data?.message || 'Failed to fetch book details');
      } finally {
        setLoading(false);
      }
    };

    fetchBookDetails();
  }, [id, isAuthenticated, navigate]);

  if (loading) {
    return <div className="container mx-auto p-4">Loading book details...</div>;
  }

  if (error) {
    return <div className="container mx-auto p-4 text-red-500">Error: {error}</div>;
  }

  if (!book) {
    return <div className="container mx-auto p-4">Book not found.</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-4">{book.title}</h1>
      <p className="text-xl text-gray-700 mb-2">by {book.authors?.join(', ')}</p>
      <p className="text-lg text-gray-600 mb-4">ISBN: {book.isbn}</p>
      {/* TODO: Add more book details and UI for updating status */}
      <button
        onClick={() => navigate('/library')}
        className="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded mt-4"
      >
        Back to Library
      </button>
    </div>
  );
};

export default BookDetail;

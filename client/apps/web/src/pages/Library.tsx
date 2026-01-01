import React, { useEffect, useState } from 'react';
import { books } from '../services/apiClient';
import { useAuth } from '../context/AuthContext';
import { useNavigate, Link } from 'react-router-dom';
import { AxiosError } from 'axios'; // Added

interface Book {
  id: string;
  title: string;
  authors: string[];
  isbn: string;
  // Add other book properties as needed
}

const Library: React.FC = () => {
  const { isAuthenticated } = useAuth(); // Removed token
  const navigate = useNavigate();
  const [bookshelfId, setBookshelfId] = useState<string>(''); // Placeholder for actual bookshelf ID
  const [userBooks, setUserBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }

    // TODO: Fetch actual bookshelfId for the authenticated user
    // For now, using a placeholder. In a real app, you'd fetch user's primary bookshelf or allow selection.
    const dummyBookshelfId = '00000000-0000-0000-0000-000000000001'; 
    setBookshelfId(dummyBookshelfId);

    const fetchBooks = async () => {
      try {
        setLoading(true);
        setError('');
        // This assumes the API endpoint for books for a bookshelf is secured and uses the token
        const fetchedBooks = await books.getBooksForBookshelf(dummyBookshelfId);
        setUserBooks(fetchedBooks);
      } catch (err: unknown) {
        const axiosError = err as AxiosError<{ message: string }>;
        setError(axiosError.response?.data?.message || 'Failed to fetch books');
      } finally {
        setLoading(false);
      }
    };

    if (bookshelfId) { // Only fetch if bookshelfId is set
        fetchBooks();
    }
  }, [isAuthenticated, navigate, bookshelfId]);

  if (loading) {
    return <div className="container mx-auto p-4">Loading books...</div>;
  }

  if (error) {
    return <div className="container mx-auto p-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">My Library</h1>
      {userBooks.length === 0 ? (
        <p>No books in your library yet. Add some!</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {userBooks.map((book) => (
            <Link to={`/book/${book.id}`} key={book.id} className="block">
              <div className="bg-white shadow-md rounded-lg p-4 hover:bg-gray-50 cursor-pointer">
                <h2 className="text-xl font-semibold">{book.title}</h2>
                <p className="text-gray-600">by {book.authors?.join(', ')}</p>
                <p className="text-gray-500">ISBN: {book.isbn}</p>
                {/* Add more book details or actions here */}
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
};

export default Library;

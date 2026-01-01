import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { books } from '../services/apiClient';
import { useAuth } from '../context/AuthContext';

const AddBook: React.FC = () => {
  const { isAuthenticated, token } = useAuth();
  const navigate = useNavigate();
  const [title, setTitle] = useState('');
  const [authors, setAuthors] = useState('');
  const [isbn, setIsbn] = useState('');
  const [publisher, setPublisher] = useState('');
  const [publishDate, setPublishDate] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  // Placeholder bookshelfId and ownerId - in a real app, these would come from context/user profile
  const bookshelfId = '00000000-0000-0000-0000-000000000001'; 
  const ownerId = '00000000-0000-0000-0000-000000000001'; // Assuming a logged in user with this ID

  if (!isAuthenticated) {
    navigate('/login');
    return null;
  }

  const handleManualSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await books.addBookManually({
        title,
        authors: authors.split(',').map(a => a.trim()),
        isbn: isbn || undefined,
        publisher: publisher || undefined,
        publishDate: publishDate || undefined,
        bookshelfId,
        ownerId,
      });
      navigate('/library');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to add book manually');
    } finally {
      setLoading(false);
    }
  };

  const handleScanIsbn = async () => {
    // TODO: Implement actual barcode scanning logic
    // For now, simulate with a prompt or pre-filled ISBN
    const scannedIsbn = prompt("Enter ISBN (e.g., 978-0321765723) for auto-fill:");
    if (!scannedIsbn) return;

    setError('');
    setLoading(true);
    try {
      // Assuming a backend endpoint for adding book via ISBN exists and pre-fills the form
      const bookData = await books.addBookFromIsbn(scannedIsbn, bookshelfId, ownerId);
      setTitle(bookData.title || '');
      setAuthors(bookData.authors?.join(', ') || '');
      setIsbn(bookData.isbn || '');
      setPublisher(bookData.publisher || '');
      setPublishDate(bookData.publishDate || '');
      alert('Book data pre-filled from ISBN scan!');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to fetch book data from ISBN');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Add New Book</h1>

      {error && <p className="text-red-500 text-xs italic mb-4">{error}</p>}

      <div className="mb-6">
        <button
          onClick={handleScanIsbn}
          className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
          disabled={loading}
        >
          {loading ? 'Scanning...' : 'Scan Barcode (ISBN) & Auto-Fill'}
        </button>
      </div>

      <form onSubmit={handleManualSubmit} className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="title">
            Title:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="title"
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="authors">
            Authors (comma-separated):
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="authors"
            type="text"
            value={authors}
            onChange={(e) => setAuthors(e.target.value)}
            required
          />
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="isbn">
            ISBN (optional):
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="isbn"
            type="text"
            value={isbn}
            onChange={(e) => setIsbn(e.target.value)}
          />
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="publisher">
            Publisher (optional):
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="publisher"
            type="text"
            value={publisher}
            onChange={(e) => setPublisher(e.target.value)}
          />
        </div>
        <div className="mb-6">
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="publishDate">
            Publish Date (YYYY-MM-DD, optional):
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="publishDate"
            type="text"
            value={publishDate}
            onChange={(e) => setPublishDate(e.target.value)}
            placeholder="YYYY-MM-DD"
          />
        </div>
        <div className="flex items-center justify-between">
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="submit"
            disabled={loading}
          >
            {loading ? 'Adding...' : 'Add Book Manually'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddBook;

import React, { useEffect, useState } from 'react';
import { Text, View, StyleSheet, ActivityIndicator, Alert } from 'react-native';
import { books } from '../src/services/apiClient'; // Adjust path as needed
import { useAuth } from '../src/context/AuthContext'; // Adjust path as needed
import { useRouter, Link } from 'expo-router';

interface Book {
  id: string;
  title: string;
  authors: string[];
  isbn: string;
  // Add other book properties as needed
}

export default function Library() {
  const { isAuthenticated, isLoading, token } = useAuth();
  const router = useRouter();
  const [bookshelfId, setBookshelfId] = useState<string>(''); // Placeholder for actual bookshelf ID
  const [userBooks, setUserBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (isLoading) {
      return; // Wait for authentication state to load
    }
    if (!isAuthenticated) {
      router.replace('/login');
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
      } catch (err: any) {
        setError(err.response?.data?.message || 'Failed to fetch books');
        Alert.alert('Error', err.response?.data?.message || 'Failed to fetch books');
      } finally {
        setLoading(false);
      }
    };

    if (bookshelfId) { // Only fetch if bookshelfId is set
        fetchBooks();
    }
  }, [isAuthenticated, isLoading, bookshelfId]);

  if (loading || isLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#0000ff" />
        <Text>Loading books...</Text>
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.container}>
        <Text style={styles.errorText}>Error: {error}</Text>
        <Link href="/" style={styles.link}>Go to Home</Link>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <Text style={styles.title}>My Library (Mobile)</Text>
      {userBooks.length === 0 ? (
        <Text>No books in your library yet. Add some!</Text>
      ) : (
        <View>
          {userBooks.map((book) => (
            <Link href={`/book/${book.id}`} key={book.id} style={styles.bookItem}>
              <Text style={styles.bookTitle}>{book.title}</Text>
              <Text style={styles.bookAuthor}>by {book.authors?.join(', ')}</Text>
              <Text style={styles.bookIsbn}>ISBN: {book.isbn}</Text>
            </Link>
          ))}
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
  },
  bookItem: {
    backgroundColor: '#f0f0f0',
    padding: 15,
    borderRadius: 8,
    marginBottom: 10,
    width: '100%',
  },
  bookTitle: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  bookAuthor: {
    fontSize: 16,
    color: '#555',
  },
  bookIsbn: {
    fontSize: 14,
    color: '#888',
  },
  errorText: {
    color: 'red',
    fontSize: 16,
    marginBottom: 10,
  },
  link: {
    marginTop: 15,
    color: 'blue',
  },
});

import React, { useEffect, useState } from 'react';
import { Text, View, StyleSheet, ActivityIndicator, Alert, Button as RNButton } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { books } from '../../src/services/apiClient';
import { useAuth } from '../../src/context/AuthContext';
import { AxiosError } from 'axios';

interface Book {
  id: string;
  title: string;
  authors: string[];
  isbn: string;
  // Add other book properties as needed
}

export default function BookDetail() {
  const { id } = useLocalSearchParams();
  const { isAuthenticated, isLoading } = useAuth();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
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

    const fetchBookDetails = async () => {
      try {
        setLoading(true);
        setError('');
        if (typeof id === 'string') {
          const fetchedBook = await books.getBookById(id);
          setBook(fetchedBook);
        }
      } catch (err) {
        if (err instanceof AxiosError) {
          const message = err.response?.data?.message;
          const msgString = message && typeof message === 'string' ? message : 'Failed to fetch book details';
          setError(msgString);
          Alert.alert('Error', msgString);
          return;
        }
      } finally {
        setLoading(false);
      }
    };

    fetchBookDetails();
  }, [id, isAuthenticated, isLoading, router]);

  const handleUpdateStatus = async (newReadingStatus?: string, newLoanStatus?: string) => {
    if (!book || !isAuthenticated) return; // TODO: Add proper ownerId logic

    // Placeholder for ownerId
    const ownerId = '00000000-0000-0000-0000-000000000001'; // TODO: This should come from authenticated user

    try {
      // Assuming a userId is associated with the token or a global context
      await books.updateUserBookStatus(book.id, ownerId, newReadingStatus, newLoanStatus);
      Alert.alert('Success', 'Book status updated!');
    } catch (err) {
      if (err instanceof AxiosError) {
        const message = err.response?.data?.message;
        const msgString = message && typeof message === 'string' ? message : 'Failed to update status';
        Alert.alert('Error', msgString);
        return;
      }
    }
  };

  if (loading || isLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#0000ff" />
        <Text>Loading book details...</Text>
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.container}>
        <Text style={styles.errorText}>Error: {error}</Text>
        <RNButton title="Go to Library" onPress={() => router.replace('/library')} />
      </View>
    );
  }

  if (!book) {
    return (
      <View style={styles.container}>
        <Text>Book not found.</Text>
        <RNButton title="Go to Library" onPress={() => router.replace('/library')} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <Text style={styles.title}>{book.title}</Text>
      <Text style={styles.author}>by {book.authors?.join(', ')}</Text>
      <Text style={styles.isbn}>ISBN: {book.isbn}</Text>
      {/* TODO: Add more book details and UI for updating status */}
      <View style={styles.buttonContainer}>
        <RNButton title="Set to Reading" onPress={() => handleUpdateStatus('Reading')} />
        <RNButton title="Set to Completed" onPress={() => handleUpdateStatus('Completed')} />
        <RNButton title="Loan Out" onPress={() => handleUpdateStatus(undefined, 'Lent')} />
        <RNButton title="Return to Library" onPress={() => handleUpdateStatus(undefined, 'None')} />
      </View>
      <RNButton title="Back to Library" onPress={() => router.replace('/library')} />
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
    marginBottom: 10,
  },
  author: {
    fontSize: 18,
    color: '#555',
    marginBottom: 5,
  },
  isbn: {
    fontSize: 16,
    color: '#888',
    marginBottom: 20,
  },
  buttonContainer: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    width: '100%',
    marginBottom: 20,
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

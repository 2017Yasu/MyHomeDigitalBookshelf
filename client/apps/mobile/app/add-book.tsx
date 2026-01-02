import React, { useState } from 'react';
import { Text, View, TextInput, Button, StyleSheet, Alert, ScrollView, ActivityIndicator } from 'react-native';
import { books } from '../src/services/apiClient';
import { useAuth } from '../src/context/AuthContext';
import { useRouter, Link } from 'expo-router';
import { AxiosError } from 'axios';

export default function AddBook() {
  const { isAuthenticated, isLoading } = useAuth();
  const router = useRouter();
  const [title, setTitle] = useState('');
  const [authors, setAuthors] = useState('');
  const [isbn, setIsbn] = useState('');
  const [publisher, setPublisher] = useState('');
  const [publishDate, setPublishDate] = useState('');
  const [loading, setLoading] = useState(false);

  // TODO: These should come from context/user profile
  const bookshelfId = '00000000-0000-0000-0000-000000000001';
  // TODO: This should come from authenticated user
  const ownerId = '00000000-0000-0000-0000-000000000001';

  if (!isAuthenticated && !isLoading) {
    router.replace('/login');
    return null;
  }

  const handleManualSubmit = async () => {
    setLoading(true);
    try {
      await books.addBookManually({
        title,
        authors: authors.split(',').map((a) => a.trim()),
        isbn: isbn || undefined,
        publisher: publisher || undefined,
        publishDate: publishDate || undefined,
        bookshelfId,
        ownerId,
      });
      Alert.alert('Success', 'Book added manually!');
      router.replace('/library');
    } catch (err) {
      if (err instanceof AxiosError) {
        const message = err.response?.data?.message;
        const msgString = message && typeof message === 'string' ? message : 'Failed to add book manually';
        Alert.alert('Error', msgString);
        return;
      }
    } finally {
      setLoading(false);
    }
  };

  const handleScanIsbn = async () => {
    // TODO: Implement actual barcode scanning logic using expo-barcode-scanner
    // For now, simulate with a prompt or pre-filled ISBN
    Alert.prompt('Scan Barcode (ISBN)', 'Enter ISBN (e.g., 978-0321765723) for auto-fill:', async (scannedIsbn) => {
      if (!scannedIsbn) return;

      setLoading(true);
      try {
        const bookData = await books.addBookFromIsbn(scannedIsbn, bookshelfId, ownerId);
        setTitle(bookData.title || '');
        setAuthors(bookData.authors?.join(', ') || '');
        setIsbn(bookData.isbn || '');
        setPublisher(bookData.publisher || '');
        setPublishDate(bookData.publishDate || '');
        Alert.alert('Success', 'Book data pre-filled from ISBN scan!');
      } catch (err) {
        if (err instanceof AxiosError) {
          const message = err.response?.data?.message;
          const msgString = message && typeof message === 'string' ? message : 'Failed to fetch book data from ISBN';
          Alert.alert('Error', msgString);
          return;
        }
      } finally {
        setLoading(false);
      }
    });
  };

  if (isLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#0000ff" />
        <Text>Loading...</Text>
      </View>
    );
  }

  return (
    <ScrollView contentContainerStyle={styles.container}>
      <Text style={styles.title}>Add New Book</Text>

      <View style={styles.buttonGroup}>
        <Button
          title={loading ? 'Scanning...' : 'Scan Barcode (ISBN) & Auto-Fill'}
          onPress={handleScanIsbn}
          disabled={loading}
        />
      </View>

      <Text style={styles.subtitle}>Or Enter Manually:</Text>

      <TextInput style={styles.input} placeholder="Title" value={title} onChangeText={setTitle} />
      <TextInput
        style={styles.input}
        placeholder="Authors (comma-separated)"
        value={authors}
        onChangeText={setAuthors}
      />
      <TextInput style={styles.input} placeholder="ISBN (optional)" value={isbn} onChangeText={setIsbn} />
      <TextInput
        style={styles.input}
        placeholder="Publisher (optional)"
        value={publisher}
        onChangeText={setPublisher}
      />
      <TextInput
        style={styles.input}
        placeholder="Publish Date (YYYY-MM-DD, optional)"
        value={publishDate}
        onChangeText={setPublishDate}
      />

      <Button title={loading ? 'Adding...' : 'Add Book Manually'} onPress={handleManualSubmit} disabled={loading} />
      <Link href="/library" style={styles.link}>
        Back to Library
      </Link>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flexGrow: 1,
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
  subtitle: {
    fontSize: 18,
    marginTop: 20,
    marginBottom: 10,
  },
  input: {
    width: '100%',
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 5,
    marginBottom: 10,
    paddingHorizontal: 10,
  },
  buttonGroup: {
    marginBottom: 20,
  },
  link: {
    marginTop: 15,
    color: 'blue',
  },
});

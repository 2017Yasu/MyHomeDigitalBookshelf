import React, { useState } from 'react';
import { View, Text, StyleSheet, Alert } from 'react-native';
import BarcodeScanner from '../../../common/ui/src/components/BarcodeScanner';
import AddBookForm from '../../../common/ui/src/components/AddBookForm';

// Mock API call for demonstration purposes
const mockFetchBookByIsbn = async (isbn: string) => {
  console.log(`Mock API call: Fetching book data for ISBN: ${isbn}`);
  return new Promise(resolve => {
    setTimeout(() => {
      if (isbn === '978-0321765723') {
        resolve({
          isbn: '978-0321765723',
          title: 'The Lord of the Rings',
          authors: ['J.R.R. Tolkien'],
          publisher: 'George Allen & Unwin',
          publishDate: '1954-07-29',
        });
      } else {
        resolve(null);
      }
    }, 1500);
  });
};

// Mock API call for adding a book
const mockAddBookApi = async (bookData: any) => {
  console.log('Mock API call: Adding book', bookData);
  return new Promise(resolve => {
    setTimeout(() => {
      resolve({ success: true, message: 'Book added successfully!' });
    }, 1000);
  });
};

const AddBookScreen: React.FC = () => {
  const [scannedIsbn, setScannedIsbn] = useState<string | null>(null);
  const [bookFormData, setBookFormData] = useState<any | null>(null);
  const [loading, setLoading] = useState(false);

  const handleBarcodeScan = async (isbn: string) => {
    setScannedIsbn(isbn);
    setLoading(true);
    const data: any = await mockFetchBookByIsbn(isbn);
    setLoading(false);

    if (data) {
      setBookFormData(data);
    } else {
      Alert.alert('Book Not Found', 'Could not find book details for the scanned ISBN. Please enter manually.');
      setBookFormData({ isbn }); // Pre-fill ISBN even if other data not found
    }
  };

  const handleFormSubmit = async (data: any) => {
    setLoading(true);
    const response: any = await mockAddBookApi(data);
    setLoading(false);

    if (response.success) {
      Alert.alert('Success', response.message);
      // Reset form or navigate away
      setScannedIsbn(null);
      setBookFormData(null);
    } else {
      Alert.alert('Error', 'Failed to add book.');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Add New Book</Text>

      {loading && <Text style={styles.loadingText}>Loading...</Text>}

      {!scannedIsbn && !loading && (
        <BarcodeScanner onScan={handleBarcodeScan} />
      )}

      {(scannedIsbn || bookFormData) && !loading && (
        <>
          <Text style={styles.subHeader}>Book Details</Text>
          <AddBookForm initialData={bookFormData} onSubmit={handleFormSubmit} />
        </>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#f0f2f5',
    alignItems: 'center',
    justifyContent: 'center',
  },
  header: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 30,
    color: '#333',
  },
  subHeader: {
    fontSize: 20,
    fontWeight: '600',
    marginTop: 20,
    marginBottom: 15,
    color: '#555',
  },
  loadingText: {
    fontSize: 18,
    color: '#007bff',
    marginBottom: 20,
  },
});

export default AddBookScreen;

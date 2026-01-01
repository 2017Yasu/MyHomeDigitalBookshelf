import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, Button, Alert } from 'react-native';
import { Picker } from '@react-native-picker/picker'; // Assuming @react-native-picker/picker is available

// Mock API call for fetching book details
const mockFetchBookDetails = async (bookId: string) => {
  console.log(`Mock API call: Fetching details for book ID: ${bookId}`);
  return new Promise(resolve => {
    setTimeout(() => {
      // Simulate fetching a book by ID
      if (bookId === '1') {
        resolve({
          id: '1',
          title: 'The Hobbit',
          authors: ['J.R.R. Tolkien'],
          isbn: '978-0321765723',
          publisher: 'George Allen & Unwin',
          publishDate: '1937-09-21',
          description: 'A classic high fantasy novel by English author J. R. R. Tolkien.',
          readingStatus: 'Reading', // Current status
          // Add more details as needed
        });
      } else {
        resolve(null);
      }
    }, 1000);
  });
};

// Mock API call for updating user book status
const mockUpdateBookStatus = async (userBookId: string, newStatus: string) => {
  console.log(`Mock API call: Updating status for user book ${userBookId} to ${newStatus}`);
  return new Promise(resolve => {
    setTimeout(() => {
      // Simulate API response
      resolve({ success: true, message: 'Status updated successfully!' });
    }, 800);
  });
};

const BookDetailScreen: React.FC = ({ route }) => { // Assuming route.params will contain bookId
  const { bookId } = route?.params || { bookId: '1' }; // Default to '1' for demonstration
  const [bookDetails, setBookDetails] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [selectedStatus, setSelectedStatus] = useState<string>('');

  useEffect(() => {
    const getBookDetails = async () => {
      setLoading(true);
      const details: any = await mockFetchBookDetails(bookId);
      setBookDetails(details);
      setSelectedStatus(details?.readingStatus || '');
      setLoading(false);
    };
    getBookDetails();
  }, [bookId]);

  const handleStatusUpdate = async () => {
    if (!bookDetails?.id || !selectedStatus) return;

    setLoading(true);
    const response: any = await mockUpdateBookStatus(bookDetails.id, selectedStatus);
    setLoading(false);

    if (response.success) {
      Alert.alert('Success', response.message);
      // Optionally, update local state or re-fetch details
      setBookDetails((prev: any) => ({ ...prev, readingStatus: selectedStatus }));
    } else {
      Alert.alert('Error', 'Failed to update reading status.');
    }
  };

  if (loading) {
    return (
      <View style={styles.loadingContainer}>
        <Text>Loading book details...</Text>
      </View>
    );
  }

  if (!bookDetails) {
    return (
      <View style={styles.loadingContainer}>
        <Text>Book not found.</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <Text style={styles.title}>{bookDetails.title}</Text>
      <Text style={styles.authors}>by {bookDetails.authors?.join(', ')}</Text>

      <View style={styles.detailRow}>
        <Text style={styles.label}>ISBN:</Text>
        <Text style={styles.value}>{bookDetails.isbn}</Text>
      </View>
      <View style={styles.detailRow}>
        <Text style={styles.label}>Publisher:</Text>
        <Text style={styles.value}>{bookDetails.publisher}</Text>
      </View>
      <View style={styles.detailRow}>
        <Text style={styles.label}>Published:</Text>
        <Text style={styles.value}>{bookDetails.publishDate}</Text>
      </View>

      <Text style={styles.descriptionHeader}>Description:</Text>
      <Text style={styles.description}>{bookDetails.description}</Text>

      <Text style={styles.statusHeader}>Update Reading Status:</Text>
      <Picker
        selectedValue={selectedStatus}
        onValueChange={(itemValue: string) => setSelectedStatus(itemValue)}
        style={styles.picker}
      >
        {/* These should match ReadingStatus enum values from backend */}
        <Picker.Item label="Want to Read" value="WantToRead" />
        <Picker.Item label="Reading" value="Reading" />
        <Picker.Item label="Completed" value="Completed" />
        <Picker.Item label="On Hold" value="OnHold" />
        <Picker.Item label="Dropped" value="Dropped" />
      </Picker>
      <Button title="Update Status" onPress={handleStatusUpdate} disabled={loading} />
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#f0f2f5',
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 5,
    color: '#333',
  },
  authors: {
    fontSize: 18,
    color: '#555',
    marginBottom: 20,
  },
  detailRow: {
    flexDirection: 'row',
    marginBottom: 10,
  },
  label: {
    fontSize: 16,
    fontWeight: 'bold',
    marginRight: 5,
    color: '#333',
  },
  value: {
    fontSize: 16,
    color: '#666',
  },
  descriptionHeader: {
    fontSize: 18,
    fontWeight: 'bold',
    marginTop: 20,
    marginBottom: 10,
    color: '#333',
  },
  description: {
    fontSize: 16,
    color: '#666',
    lineHeight: 24,
  },
  statusHeader: {
    fontSize: 18,
    fontWeight: 'bold',
    marginTop: 20,
    marginBottom: 10,
    color: '#333',
  },
  picker: {
    height: 50,
    width: '100%',
    marginBottom: 20,
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 4,
  },
});

export default BookDetailScreen;

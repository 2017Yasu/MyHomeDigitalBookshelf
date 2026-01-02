import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ActivityIndicator } from 'react-native';
import { BookList, FilterSort } from '@mhdb/ui';

// Mock API call for fetching books
const mockFetchBooks = async (filters: any, sortBy: string) => {
  console.log('Mock API call: Fetching books with filters', filters, 'and sortBy', sortBy);
  return new Promise((resolve) => {
    setTimeout(() => {
      const allBooks = [
        { id: '1', title: 'The Hobbit', authors: ['J.R.R. Tolkien'], coverImageUrl: 'https://example.com/hobbit.jpg' },
        {
          id: '2',
          title: 'The Lord of the Rings',
          authors: ['J.R.R. Tolkien'],
          coverImageUrl: 'https://example.com/lotr.jpg',
        },
        {
          id: '3',
          title: 'Pride and Prejudice',
          authors: ['Jane Austen'],
          coverImageUrl: 'https://example.com/pride.jpg',
        },
        {
          id: '4',
          title: 'To Kill a Mockingbird',
          authors: ['Harper Lee'],
          coverImageUrl: 'https://example.com/mockingbird.jpg',
        },
      ];

      // Simple filtering logic
      let filteredBooks = allBooks.filter((book) => {
        let match = true;
        if (filters.title && !book.title.toLowerCase().includes(filters.title.toLowerCase())) {
          match = false;
        }
        if (filters.author && !book.authors.some((a) => a.toLowerCase().includes(filters.author.toLowerCase()))) {
          match = false;
        }
        // Add more filter logic here (ISBN, ReadingStatus, etc.)
        return match;
      });

      // Simple sorting logic
      filteredBooks.sort((a, b) => {
        if (sortBy === 'TitleAsc') return a.title.localeCompare(b.title);
        if (sortBy === 'TitleDesc') return b.title.localeCompare(a.title);
        // Add more sorting logic here
        return 0;
      });

      resolve(filteredBooks);
    }, 1500);
  });
};

const LibraryScreen: React.FC = () => {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filters, setFilters] = useState({});
  const [sortBy, setSortBy] = useState('TitleAsc');

  const fetchBooks = async (currentFilters: any, currentSortBy: string) => {
    setLoading(true);
    const fetchedBooks: any = await mockFetchBooks(currentFilters, currentSortBy);
    setBooks(fetchedBooks);
    setLoading(false);
  };

  useEffect(() => {
    fetchBooks(filters, sortBy);
  }, [filters, sortBy]);

  const handleApplyFilters = (newFilters: any) => {
    setFilters(newFilters);
  };

  const handleApplySort = (newSortBy: string) => {
    setSortBy(newSortBy);
  };

  const handleBookPress = (book: any) => {
    // Navigate to Book Detail Screen
    console.log('Navigating to book detail for:', book.title);
    // You would use navigation.navigate('BookDetail', { bookId: book.id }); here
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>My Library</Text>

      <View style={styles.filterSortContainer}>
        <FilterSort
          onApplyFilters={handleApplyFilters}
          onApplySort={handleApplySort}
          initialFilters={filters}
          initialSortBy={sortBy}
        />
      </View>

      {loading ? (
        <ActivityIndicator size="large" color="#0000ff" />
      ) : (
        <BookList books={books} onBookPress={handleBookPress} />
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
  },
  header: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 20,
    color: '#333',
  },
  filterSortContainer: {
    width: '100%',
    marginBottom: 20,
  },
});

export default LibraryScreen;

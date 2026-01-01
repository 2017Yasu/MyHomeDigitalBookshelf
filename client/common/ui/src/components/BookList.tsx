import React from 'react';
import { View, Text, StyleSheet, FlatList, TouchableOpacity } from 'react-native';

interface Book {
  id: string;
  title: string;
  authors: string[];
  coverImageUrl?: string;
  // Add other relevant book properties
}

interface BookListItemProps {
  book: Book;
  onPress: (book: Book) => void;
}

const BookListItem: React.FC<BookListItemProps> = ({ book, onPress }) => {
  return (
    <TouchableOpacity style={styles.itemContainer} onPress={() => onPress(book)}>
      {book.coverImageUrl ? (
        // <Image source={{ uri: book.coverImageUrl }} style={styles.coverImage} />
        <View style={styles.coverImagePlaceholder}>
          <Text style={styles.coverImageText}>Cover</Text>
        </View>
      ) : (
        <View style={styles.coverImagePlaceholder}>
          <Text style={styles.coverImageText}>No Cover</Text>
        </View>
      )}
      <View style={styles.infoContainer}>
        <Text style={styles.title}>{book.title}</Text>
        <Text style={styles.authors}>{book.authors.join(', ')}</Text>
      </View>
    </TouchableOpacity>
  );
};

interface BookListProps {
  books: Book[];
  onBookPress: (book: Book) => void;
  // Add props for pagination, loading state, etc.
}

const BookList: React.FC<BookListProps> = ({ books, onBookPress }) => {
  const renderItem = ({ item }: { item: Book }) => (
    <BookListItem book={item} onPress={onBookPress} />
  );

  return (
    <View style={styles.listContainer}>
      <FlatList
        data={books}
        renderItem={renderItem}
        keyExtractor={(item) => item.id}
        showsVerticalScrollIndicator={false}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  listContainer: {
    flex: 1,
    width: '100%',
  },
  itemContainer: {
    flexDirection: 'row',
    backgroundColor: '#fff',
    borderRadius: 8,
    marginVertical: 8,
    padding: 10,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.2,
    shadowRadius: 1.41,
    elevation: 2,
  },
  coverImagePlaceholder: {
    width: 60,
    height: 90,
    backgroundColor: '#e0e0e0',
    borderRadius: 4,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 10,
  },
  coverImageText: {
    fontSize: 10,
    color: '#666',
  },
  infoContainer: {
    flex: 1,
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  authors: {
    fontSize: 14,
    color: '#666',
    marginTop: 4,
  },
});

export default BookList;

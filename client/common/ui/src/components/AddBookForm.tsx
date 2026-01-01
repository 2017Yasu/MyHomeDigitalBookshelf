import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, Button, StyleSheet } from 'react-native';

interface AddBookFormProps {
  initialData?: {
    isbn?: string;
    title?: string;
    authors?: string[];
    publisher?: string;
    publishDate?: string;
    // Add other relevant book fields
  };
  onSubmit: (bookData: any) => void; // Define a more specific type for bookData
}

const AddBookForm: React.FC<AddBookFormProps> = ({ initialData, onSubmit }) => {
  const [isbn, setIsbn] = useState(initialData?.isbn || '');
  const [title, setTitle] = useState(initialData?.title || '');
  const [authors, setAuthors] = useState(initialData?.authors?.join(', ') || '');
  const [publisher, setPublisher] = useState(initialData?.publisher || '');
  const [publishDate, setPublishDate] = useState(initialData?.publishDate || '');
  // Add state for other fields as needed

  useEffect(() => {
    if (initialData) {
      setIsbn(initialData.isbn || '');
      setTitle(initialData.title || '');
      setAuthors(initialData.authors?.join(', ') || '');
      setPublisher(initialData.publisher || '');
      setPublishDate(initialData.publishDate || '');
    }
  }, [initialData]);

  const handleSubmit = () => {
    const bookData = {
      isbn,
      title,
      authors: authors.split(',').map(a => a.trim()).filter(a => a),
      publisher,
      publishDate,
      // Include other field values
    };
    onSubmit(bookData);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Add Book Details</Text>
      <TextInput
        style={styles.input}
        placeholder="ISBN"
        value={isbn}
        onChangeText={setIsbn}
        editable={!initialData?.isbn} // Make ISBN editable if not pre-filled
      />
      <TextInput
        style={styles.input}
        placeholder="Title"
        value={title}
        onChangeText={setTitle}
      />
      <TextInput
        style={styles.input}
        placeholder="Authors (comma-separated)"
        value={authors}
        onChangeText={setAuthors}
      />
      <TextInput
        style={styles.input}
        placeholder="Publisher"
        value={publisher}
        onChangeText={setPublisher}
      />
      <TextInput
        style={styles.input}
        placeholder="Publication Date (YYYY-MM-DD)"
        value={publishDate}
        onChangeText={setPublishDate}
      />
      {/* Add more input fields as per your Book entity */}
      <Button title="Add Book" onPress={handleSubmit} />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    padding: 20,
    backgroundColor: '#fff',
    borderRadius: 8,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 8,
    elevation: 5,
  },
  title: {
    fontSize: 22,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
  },
  input: {
    height: 40,
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 4,
    marginBottom: 15,
    paddingHorizontal: 10,
    backgroundColor: '#f9f9f9',
  },
});

export default AddBookForm;

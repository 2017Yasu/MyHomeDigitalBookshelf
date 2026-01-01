import React, { useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal, TextInput, ScrollView } from 'react-native';
import { Picker } from '@react-native-picker/picker'; // Assuming @react-native-picker/picker is available

interface FilterSortProps {
  onApplyFilters: (filters: any) => void; // Define a more specific type for filters
  onApplySort: (sortBy: string) => void;
  initialFilters?: any;
  initialSortBy?: string;
}

const FilterSort: React.FC<FilterSortProps> = ({
  onApplyFilters,
  onApplySort,
  initialFilters = {},
  initialSortBy = 'TitleAsc',
}) => {
  const [modalVisible, setModalVisible] = useState(false);
  const [filters, setFilters] = useState(initialFilters);
  const [sortBy, setSortBy] = useState(initialSortBy);

  const handleFilterChange = (key: string, value: string | undefined) => {
    setFilters((prev: any) => ({ ...prev, [key]: value }));
  };

  const handleApply = () => {
    onApplyFilters(filters);
    onApplySort(sortBy);
    setModalVisible(false);
  };

  const handleClear = () => {
    setFilters({});
    setSortBy('TitleAsc');
    onApplyFilters({});
    onApplySort('TitleAsc');
    setModalVisible(false);
  };

  return (
    <View style={styles.container}>
      <TouchableOpacity onPress={() => setModalVisible(true)} style={styles.button}>
        <Text style={styles.buttonText}>Filter & Sort</Text>
      </TouchableOpacity>

      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Filter & Sort Options</Text>
            <ScrollView>
              {/* Filters */}
              <Text style={styles.sectionTitle}>Filters</Text>
              <TextInput
                style={styles.input}
                placeholder="Title"
                value={filters.title || ''}
                onChangeText={(text) => handleFilterChange('title', text)}
              />
              <TextInput
                style={styles.input}
                placeholder="Author"
                value={filters.author || ''}
                onChangeText={(text) => handleFilterChange('author', text)}
              />
              <TextInput
                style={styles.input}
                placeholder="ISBN"
                value={filters.isbn || ''}
                onChangeText={(text) => handleFilterChange('isbn', text)}
              />

              {/* Reading Status Filter */}
              <Text style={styles.label}>Reading Status:</Text>
              <Picker
                selectedValue={filters.readingStatus || ''}
                onValueChange={(itemValue) => handleFilterChange('readingStatus', itemValue)}
                style={styles.picker}
              >
                <Picker.Item label="Any" value="" />
                <Picker.Item label="Want to Read" value="WantToRead" />
                <Picker.Item label="Reading" value="Reading" />
                <Picker.Item label="Completed" value="Completed" />
                <Picker.Item label="On Hold" value="OnHold" />
                <Picker.Item label="Dropped" value="Dropped" />
              </Picker>


              {/* Sort By */}
              <Text style={styles.sectionTitle}>Sort By</Text>
              <Picker
                selectedValue={sortBy}
                onValueChange={(itemValue) => setSortBy(itemValue)}
                style={styles.picker}
              >
                <Picker.Item label="Title (A-Z)" value="TitleAsc" />
                <Picker.Item label="Title (Z-A)" value="TitleDesc" />
                <Picker.Item label="Author (A-Z)" value="AuthorAsc" />
                <Picker.Item label="Author (Z-A)" value="AuthorDesc" />
                <Picker.Item label="Newest First" value="NewestFirst" />
                <Picker.Item label="Oldest First" value="OldestFirst" />
              </Picker>

              <View style={styles.buttonGroup}>
                <TouchableOpacity onPress={handleApply} style={[styles.button, styles.applyButton]}>
                  <Text style={styles.buttonText}>Apply</Text>
                </TouchableOpacity>
                <TouchableOpacity onPress={handleClear} style={[styles.button, styles.clearButton]}>
                  <Text style={styles.buttonText}>Clear</Text>
                </TouchableOpacity>
                <TouchableOpacity onPress={() => setModalVisible(false)} style={[styles.button, styles.cancelButton]}>
                  <Text style={styles.buttonText}>Cancel</Text>
                </TouchableOpacity>
              </View>
            </ScrollView>
          </View>
        </View>
      </Modal>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    // Styling for the button that opens the modal
    margin: 10,
  },
  button: {
    backgroundColor: '#007bff',
    padding: 12,
    borderRadius: 8,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  modalOverlay: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    width: '90%',
    maxHeight: '80%',
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
  },
  modalTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginTop: 15,
    marginBottom: 10,
  },
  label: {
    fontSize: 16,
    marginBottom: 5,
    color: '#333',
  },
  input: {
    height: 40,
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 4,
    paddingHorizontal: 10,
    marginBottom: 10,
  },
  picker: {
    height: 50,
    width: '100%',
    marginBottom: 10,
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 4,
  },
  buttonGroup: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginTop: 20,
  },
  applyButton: {
    backgroundColor: '#28a745',
    flex: 1,
    marginHorizontal: 5,
  },
  clearButton: {
    backgroundColor: '#ffc107',
    flex: 1,
    marginHorizontal: 5,
  },
  cancelButton: {
    backgroundColor: '#6c757d',
    flex: 1,
    marginHorizontal: 5,
  },
});

export default FilterSort;

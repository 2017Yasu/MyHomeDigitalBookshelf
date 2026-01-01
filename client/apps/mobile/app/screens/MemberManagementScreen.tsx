import React, { useState } from 'react';
import { View, Text, StyleSheet, TextInput, Button, Alert, ScrollView } from 'react-native';

// Mock API call for inviting a user
const mockInviteUserApi = async (bookshelfId: string, email: string) => {
  console.log(`Mock API call: Inviting ${email} to bookshelf ${bookshelfId}`);
  return new Promise(resolve => {
    setTimeout(() => {
      if (email.includes('@')) {
        resolve({ success: true, message: `Invitation sent to ${email}` });
      } else {
        resolve({ success: false, message: 'Invalid email format' });
      }
    }, 1500);
  });
};

const MemberManagementScreen: React.FC = ({ route }) => { // Assuming route.params will contain bookshelfId
  const { bookshelfId } = route?.params || { bookshelfId: 'some-bookshelf-id' }; // Default for demonstration
  const [invitedEmail, setInvitedEmail] = useState('');
  const [loading, setLoading] = useState(false);
  const [members, setMembers] = useState<any[]>([ // Placeholder for existing members
    { id: '1', email: 'admin@example.com', role: 'Administrator' },
    { id: '2', email: 'member1@example.com', role: 'Member' },
  ]);

  const handleInviteUser = async () => {
    if (!invitedEmail) {
      Alert.alert('Error', 'Please enter an email address.');
      return;
    }

    setLoading(true);
    const response: any = await mockInviteUserApi(bookshelfId, invitedEmail);
    setLoading(false);

    if (response.success) {
      Alert.alert('Success', response.message);
      setInvitedEmail('');
      // Optionally, refresh members list or add the invited user optimistically
      setMembers([...members, { id: Math.random().toString(), email: invitedEmail, role: 'Pending' }]);
    } else {
      Alert.alert('Error', response.message);
    }
  };

  return (
    <ScrollView style={styles.container}>
      <Text style={styles.header}>Bookshelf Member Management</Text>
      <Text style={styles.subHeader}>Bookshelf ID: {bookshelfId}</Text>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Invite New Member</Text>
        <TextInput
          style={styles.input}
          placeholder="Enter email to invite"
          keyboardType="email-address"
          value={invitedEmail}
          onChangeText={setInvitedEmail}
          autoCapitalize="none"
        />
        <Button title="Send Invitation" onPress={handleInviteUser} disabled={loading} />
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Current Members</Text>
        {members.map(member => (
          <View key={member.id} style={styles.memberItem}>
            <Text style={styles.memberEmail}>{member.email}</Text>
            <Text style={styles.memberRole}>{member.role}</Text>
          </View>
        ))}
        {members.length === 0 && <Text>No members yet.</Text>}
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#f0f2f5',
  },
  header: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 10,
    color: '#333',
    textAlign: 'center',
  },
  subHeader: {
    fontSize: 16,
    color: '#666',
    marginBottom: 20,
    textAlign: 'center',
  },
  section: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 15,
    marginBottom: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.2,
    shadowRadius: 1.41,
    elevation: 2,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 15,
    color: '#333',
  },
  input: {
    height: 45,
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 6,
    paddingHorizontal: 12,
    marginBottom: 15,
    backgroundColor: '#f9f9f9',
  },
  memberItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 10,
    borderBottomWidth: 1,
    borderBottomColor: '#eee',
  },
  memberEmail: {
    fontSize: 16,
    color: '#333',
  },
  memberRole: {
    fontSize: 14,
    color: '#888',
    fontStyle: 'italic',
  },
});

export default MemberManagementScreen;

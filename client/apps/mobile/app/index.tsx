import { Text, View } from 'react-native';
import { Link } from 'expo-router';

export default function Home() {
  return (
    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
      <Text>Welcome to My Home Digital Bookshelf (Mobile)</Text>
      <Link href="/login">Go to Login</Link>
      <Link href="/register">Go to Register</Link>
      <Link href="/library">Go to Library</Link>
      <Link href="/add-book">Add Book</Link>
    </View>
  );
}

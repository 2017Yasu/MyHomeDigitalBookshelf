import { SafeAreaProvider } from 'react-native-safe-area-context';
import { Stack } from 'expo-router';
import '../global.css';
import '../translation';

export default function Layout() {
  return (
    <SafeAreaProvider>
      <Stack />
    </SafeAreaProvider>
  );
}


  import '../global.css';
  import { SafeAreaProvider } from 'react-native-safe-area-context';




  import '../translation';

	import { Stack } from "expo-router";



export default function Layout() {
  

	return (
    
      <SafeAreaProvider>
        <Stack />
      </SafeAreaProvider>
    
	);
}

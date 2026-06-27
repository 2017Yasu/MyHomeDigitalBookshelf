import { Stack } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { Text, View } from 'react-native';

export default function Home() {
  const { t } = useTranslation();
  return (
    <View className={styles.container}>
      <Stack.Screen options={{ title: t('home.title') }} />
      <Text className="mt-4 text-center text-2xl font-bold">{t('home.title')}</Text>
    </View>
  );
}

const styles = {
  container: 'flex flex-1 bg-white',
};

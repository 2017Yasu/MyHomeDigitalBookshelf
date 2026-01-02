import { type ConfigContext } from 'expo/config';

export default ({ config }: ConfigContext) => {
  const appEnv = process.env.APP_ENV || 'dev';
  const apiBaseUrl = (appEnv !== 'dev' ? config.extra?.API_BASE_URL : null) ?? 'http://localhost:5262/api/v1';

  return {
    ...config,
    extra: {
      ...config.extra,
      API_BASE_URL: apiBaseUrl,
    },
  };
};

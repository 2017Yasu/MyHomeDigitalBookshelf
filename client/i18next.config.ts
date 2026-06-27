import { defineConfig } from 'i18next-cli';

export default defineConfig({
  locales: ['en', 'fr'],
  extract: {
    input: ['{app,components}/**/*.{js,jsx,ts,tsx}'],
    output: 'translation/{{language}}.json',
    defaultNS: false,
  },
});

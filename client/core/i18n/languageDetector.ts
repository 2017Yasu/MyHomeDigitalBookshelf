import * as Localization from 'expo-localization';
import { LanguageDetectorModule } from 'i18next';

export const languageDetector: LanguageDetectorModule = {
  type: 'languageDetector',
  detect: () => {
    const locales = Localization.getLocales();
    let firstLanguageCode = 'en';
    if (locales && locales.length > 0) {
      firstLanguageCode = locales[0].languageCode ?? firstLanguageCode;
    }
    return firstLanguageCode;
  },
  init: () => {},
  cacheUserLanguage: () => {},
};

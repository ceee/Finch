
import { defineStore } from 'pinia';

export type TranslationState = {
  culture: string,
  translations: Record<string, string>
};

export const useTranslationStore = defineStore('zero.translations', {
  state: () => ({
    culture: null,
    translations: { name: 'Mini' }
  } as TranslationState),

  actions: {
    find(key: string): string
    {
      return this.translations[key.toLowerCase()];
    }
  }
});

import { defineStore } from 'pinia';

export type TranslationState = {
  culture: string,
  translations: Record<string, string>
};

export const useTranslationStore = defineStore('zero.translations', {
  state: () => ({
    culture: null,
    translations: {}
  } as TranslationState),

  getters: {
    getTranslation(key: string): string
    {
      return this.state.translations[key.toLowerCase()];
    }
  },

  actions: {}
});
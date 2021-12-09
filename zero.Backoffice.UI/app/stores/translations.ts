
import { defineStore } from 'pinia';
import uiApi from '../ui/api';

export type TranslationState = {
  culture: string,
  translations: Record<string, string>
};

export const useTranslationStore = defineStore('zero.translations', {
  state: () => ({
    culture: null,
    translations: { }
  } as TranslationState),

  actions: {
    find(key: string): string
    {
      return this.translations[key.toLowerCase()];
    },

    async setup()
    {
      this.translations = await uiApi.getTranslations();
    }
  }
});
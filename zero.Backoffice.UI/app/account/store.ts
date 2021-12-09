
import { defineStore } from 'pinia';
import { AccountStoreState } from 'zero/account';

export type TranslationState = {
  culture: string,
  translations: Record<string, string>
};

export const useAccountStore = defineStore('zero.account', {
  state: () => ({
    user: undefined
  } as AccountStoreState),

  getters: {
    isAuthenticated: state => state.user != null
  }
});

import { defineStore } from 'pinia';
import { AccountStoreState } from 'zero/account';

export const useAccountStore = defineStore('zero.account', {
  state: () => ({
    user: undefined
  } as AccountStoreState),

  getters: {
    isAuthenticated: state => state.user != null
  }
});

import { defineStore } from 'pinia';

const SEARCH_BY_SHORTCUT_KEY = 'zero.search.shortcut';

export const useSearchStore = defineStore('zero.search', {
  state: () => ({}),

  actions: {
    shortcutEnabled()
    {
      return localStorage.getItem(SEARCH_BY_SHORTCUT_KEY) !== 'false';
    },

    setShortcutEnabled(enabled: boolean)
    {
      localStorage.setItem(SEARCH_BY_SHORTCUT_KEY, enabled.toString());
    }
  }
});
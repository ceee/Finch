
import { defineStore } from 'pinia';
import api from './api';

export const useAppStore = defineStore('zero.apps', {
  state: () => ({
    appId: null,
    applications: []
  }),

  actions: {
    async setup()
    {
      const response = await api.getByQuery({ pageSize: 1000 });
      this.applications = response.data;
      this.appId = this.applications[0].id;
    }
  }
});

import { defineStore } from 'pinia';
import { UiIconSet, UiSection, UiSettingsGroup, UiStoreState } from 'zero/ui';
import api from './api';

export const useUiStore = defineStore('zero.ui', {
  state: () => ({
    sections: [],
    settingGroups: [],
    iconSets: []
  } as UiStoreState),

  actions: {
    async setup()
    {
      const values = await Promise.all([
        api.getSections(),
        api.getSettingGroups(),
        api.getIconSets()
      ]);

      this.sections = values[0].data as UiSection[];
      this.settingGroups = values[1].data as UiSettingsGroup[];
      this.iconSets = values[2].data as UiIconSet[];
    }
  }
});
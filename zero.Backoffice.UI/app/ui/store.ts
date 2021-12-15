
import { defineStore } from 'pinia';
import { UiFlavorProvider, UiIconSet, UiSection, UiSettingsGroup, UiStoreState } from 'zero/ui';
import api from './api';

export const useUiStore = defineStore('zero.ui', {
  state: () => ({
    sections: [],
    settingGroups: [],
    iconSets: [],
    flavors: [],
    blueprints: []
  } as UiStoreState),

  actions: {
    async setup()
    {
      const values = await Promise.all([
        api.getSections(),
        api.getSettingGroups(),
        api.getIconSets(),
        api.getFlavors(),
        api.getBlueprints()
      ]);

      this.sections = values[0].data as UiSection[];
      this.settingGroups = values[1].data as UiSettingsGroup[];
      this.iconSets = values[2].data as UiIconSet[];
      this.flavors = values[3].data as Record<string, UiFlavorProvider>;
      this.blueprints = values[4].data as string[];
    }
  }
});
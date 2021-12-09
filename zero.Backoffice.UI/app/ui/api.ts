import { UiIconSet, UiSection, UiSettingsGroup } from 'zero/ui';
import { get, ZeroRequestConfig } from '../services/request';

export default {

  getSections: async (config?: ZeroRequestConfig): Promise<UiSection[]> => await get('backoffice/ui/sections', { ...config }),

  getSettingGroups: async (config?: ZeroRequestConfig): Promise<UiSettingsGroup[]> => await get('backoffice/ui/settingareas', { ...config }),

  getIconSets: async (config?: ZeroRequestConfig): Promise<UiIconSet[]> => await get('backoffice/ui/iconsets', { ...config }),

  getTranslations: async (config?: ZeroRequestConfig): Promise<Record<string, string>> => await get('backoffice/ui/translations', { ...config }),
};
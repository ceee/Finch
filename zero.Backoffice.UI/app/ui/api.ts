import { UiIconSet, UiSection, UiSettingsGroup } from 'zero/ui';
import { get, ApiResponse, ApiRequestConfig } from '../services/request';

export default {

  getSections: async (config?: ApiRequestConfig): Promise<ApiResponse<UiSection[]>> => await get('backoffice/ui/sections', { ...config }),

  getSettingGroups: async (config?: ApiRequestConfig): Promise<ApiResponse<UiSettingsGroup[]>> => await get('backoffice/ui/settingareas', { ...config }),

  getIconSets: async (config?: ApiRequestConfig): Promise<ApiResponse<UiIconSet[]>> => await get('backoffice/ui/iconsets', { ...config }),

  getTranslations: async (config?: ApiRequestConfig): Promise<ApiResponse<Record<string, string>>> => await get('backoffice/ui/translations', { ...config }),
};
import { UiFlavorProvider, UiIconSet, UiSection, UiSettingsGroup } from 'zero/ui';
import { get, ApiResponse, ApiRequestConfig } from '../services/request';

export default {

  getSections: (config?: ApiRequestConfig): Promise<ApiResponse<UiSection[]>> => get('backoffice/ui/sections', { ...config }),

  getSettingGroups: (config?: ApiRequestConfig): Promise<ApiResponse<UiSettingsGroup[]>> => get('backoffice/ui/settingareas', { ...config }),

  getIconSets: (config?: ApiRequestConfig): Promise<ApiResponse<UiIconSet[]>> => get('backoffice/ui/iconsets', { ...config }),

  getTranslations: (config?: ApiRequestConfig): Promise<ApiResponse<Record<string, string>>> => get('backoffice/ui/translations', { ...config }),

  getFlavors: (config?: ApiRequestConfig): Promise<ApiResponse<Record<string, UiFlavorProvider>>> => get('backoffice/ui/flavors', { ...config }),

  getBlueprints: (config?: ApiRequestConfig): Promise<ApiResponse<string[]>> => get('backoffice/ui/blueprints', { ...config }),
};
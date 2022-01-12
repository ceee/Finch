import { get, ApiRequestConfig } from '../../services/request';

export default {

  getEmpty: (alias: string, config?: ApiRequestConfig) => get(`pagemodules/${alias}/empty`, { ...config }),

  getTypes: (pageId?: string, tags?: string[], config?: ApiRequestConfig) => get('pagemodules', { ...config, params: { pageId, tags } }),

  getType: (alias: string, config?: ApiRequestConfig) => get('pagemodules/' + alias, { ...config })
};
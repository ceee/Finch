import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getTypes: (config?: ApiRequestConfig) => get('integrations/types', { ...config }),

  getType: (alias: string, config?: ApiRequestConfig) => get('integrations/types/' + alias, { ...config }),

  //getEmpty: async alias => await get(base + 'getEmpty', { params: { alias } }),
  //getTypes: async () => await get(base + 'getTypes'),
  //getByAlias: async (alias, config) => await get(base + 'getByAlias', { ...config, params: { alias } }),
  //getByQuery: async (query, config) => await get(base + 'getByQuery', { ...config, params: { query } }),
  //save: async (model, config) => await post(base + 'save', model, { ...config }),
  //saveActiveState: async (model, config) => await post(base + 'saveActiveState', model, { ...config }),
  //delete: async alias => await del(base + 'delete', { params: { alias } })
};
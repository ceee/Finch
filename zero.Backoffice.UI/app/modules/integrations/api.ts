import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("translations/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('translations/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('translations', { ...config, params: { ...query } }),

  create: (model: any, config?: ApiRequestConfig) => post('translations', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('translations/' + model.id, model, config),

  delete: (id: string, config?: ApiRequestConfig) => del('translations/' + id, null, config),

  //getEmpty: async alias => await get(base + 'getEmpty', { params: { alias } }),
  //getTypes: async () => await get(base + 'getTypes'),
  //getByAlias: async (alias, config) => await get(base + 'getByAlias', { ...config, params: { alias } }),
  //getByQuery: async (query, config) => await get(base + 'getByQuery', { ...config, params: { query } }),
  //save: async (model, config) => await post(base + 'save', model, { ...config }),
  //saveActiveState: async (model, config) => await post(base + 'saveActiveState', model, { ...config }),
  //delete: async alias => await del(base + 'delete', { params: { alias } })
};
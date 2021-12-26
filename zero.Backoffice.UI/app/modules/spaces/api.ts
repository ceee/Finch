import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getTypes: (config?: ApiRequestConfig) => get('spaces/types', { ...config }),

  getType: (alias: string, config?: ApiRequestConfig) => get('spaces/types/' + alias, { ...config }),

  getByAlias: (alias: string, query?: ApiRequestQuery, config?: ApiRequestConfig) => get('spaces/' + alias, { ...config, params: { ...(query || {}) }}),

  getEmpty: (alias: string, config?: ApiRequestConfig) => get("spaces/empty", { ...config, params: { alias } }),


  //getByAlias: async alias => await get(base + 'getByAlias', { params: { alias } }),

  //getAll: async () => await get(base + 'getAll'),

  //getList: async (alias, query) =>
  //{
  //  query.alias = alias;
  //  return await get(base + 'getList', { params: query })
  //},

  //getContent: async (alias, contentId) => await get(base + 'getContent', { params: { alias, contentId } }),

  //save: async model => await post(base + 'save', model),

  //delete: async (alias, id) => await del(base + 'delete', { params: { alias, id } })
};
import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: async (flavor?: string, config?: ApiRequestConfig) => await get("countries/empty", { ...config, params: { flavor } }),

  getById: async (id: string, changeVector?: string, config?: ApiRequestConfig) => await get('countries/' + id, { ...config, params: { changeVector } }),

  getByQuery: async (query: ApiRequestQuery, config?: ApiRequestConfig) => await get('countries', { ...config, params: { ...query } }),

  create: async (model: any, config?: ApiRequestConfig) => await post('countries', model, config),

  update: async (model: any, config?: ApiRequestConfig) => await put('countries/' + model.id, model, config),

  delete: async (id: string, config?: ApiRequestConfig) => await del('countries/' + id, config),
};
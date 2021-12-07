import { get, post, put, del, ZeroRequestConfig, ZeroRequestQuery } from '../../services/request';

export default {

  getEmpty: async (flavor?: string, config?: ZeroRequestConfig) => await get("countries/empty", { ...config, params: { flavor } }),

  getById: async (id: string, changeVector?: string, config?: ZeroRequestConfig) => await get('countries/' + id, { ...config, params: { changeVector } }),

  getByQuery: async (query: ZeroRequestQuery, config?: ZeroRequestConfig) => await get('countries', { ...config, params: { ...query } }),

  create: async (model: any, config?: ZeroRequestConfig) => await post('countries', model, config),

  update: async (model: any, config?: ZeroRequestConfig) => await put('countries/' + model.id, model, config),

  delete: async (id: string, config?: ZeroRequestConfig) => await del('countries/' + id, config),
};
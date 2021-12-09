import { get, post, put, del, ZeroRequestConfig, ZeroRequestQuery } from '../../services/request';

export default {

  getEmpty: async (flavor?: string, config?: ZeroRequestConfig) => await get("applications/empty", { ...config, params: { flavor } }),

  getById: async (id: string, changeVector?: string, config?: ZeroRequestConfig) => await get('applications/' + id, { ...config, params: { changeVector } }),

  getByQuery: async (query: ZeroRequestQuery, config?: ZeroRequestConfig) => await get('applications', { ...config, params: { ...query } }),

  create: async (model: any, config?: ZeroRequestConfig) => await post('applications', model, config),

  update: async (model: any, config?: ZeroRequestConfig) => await put('applications/' + model.id, model, config),

  delete: async (id: string, config?: ZeroRequestConfig) => await del('applications/' + id, config),
};
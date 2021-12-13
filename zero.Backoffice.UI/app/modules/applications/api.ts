import { get, post, put, del, ZeroRequestConfig, ZeroRequestQuery } from '../../services/request';

export default {

  getEmpty: async (flavor?: string, config?: ZeroRequestConfig) => await get("applications/empty", { ...config, params: { flavor }, system: true }),

  getById: async (id: string, changeVector?: string, config?: ZeroRequestConfig) => await get('applications/' + id, { ...config, params: { changeVector }, system: true }),

  getByQuery: async (query: ZeroRequestQuery, config?: ZeroRequestConfig) => await get('applications', { ...config, params: { ...query }, system: true }),

  create: async (model: any, config?: ZeroRequestConfig) => await post('applications', model, { ...config, system: true }),

  update: async (model: any, config?: ZeroRequestConfig) => await put('applications/' + model.id, model, { ...config, system: true }),

  delete: async (id: string, config?: ZeroRequestConfig) => await del('applications/' + id, { ...config, system: true }),
};
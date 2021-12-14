import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: async (flavor?: string, config?: ApiRequestConfig) => await get("applications/empty", { ...config, params: { flavor }, system: true }),

  getById: async (id: string, changeVector?: string, config?: ApiRequestConfig) => await get('applications/' + id, { ...config, params: { changeVector }, system: true }),

  getByQuery: async (query: ApiRequestQuery, config?: ApiRequestConfig) => await get('applications', { ...config, params: { ...query }, system: true }),

  create: async (model: any, config?: ApiRequestConfig) => await post('applications', model, { ...config, system: true }),

  update: async (model: any, config?: ApiRequestConfig) => await put('applications/' + model.id, model, { ...config, system: true }),

  delete: async (id: string, config?: ApiRequestConfig) => await del('applications/' + id, { ...config, system: true }),
};
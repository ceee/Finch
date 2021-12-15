import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("languages/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('languages/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('languages', { ...config, params: { ...query } }),

  create: (model: any, config?: ApiRequestConfig) => post('languages', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('languages/' + model.id, model, config),

  delete: (id: string, config?: ApiRequestConfig) => del('languages/' + id, config),
};
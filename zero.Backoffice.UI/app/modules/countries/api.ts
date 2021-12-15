import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("countries/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('countries/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('countries', { ...config, params: { ...query } }),

  create: (model: any, config?: ApiRequestConfig) => post('countries', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('countries/' + model.id, model, config),

  delete: (id: string, config?: ApiRequestConfig) => del('countries/' + id, config),
};
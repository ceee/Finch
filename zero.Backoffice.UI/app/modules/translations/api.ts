import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("translations/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('translations/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('translations', { ...config, params: { ...query } }),

  create: (model: any, config?: ApiRequestConfig) => post('translations', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('translations/' + model.id, model, config),

  delete: (id: string, config?: ApiRequestConfig) => del('translations/' + id, null, config),
};
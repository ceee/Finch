import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("mailtemplates/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('mailtemplates/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('mailtemplates', { ...config, params: { ...query } }),

  create: (model: any, config?: ApiRequestConfig) => post('mailtemplates', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('mailtemplates/' + model.id, model, config),

  delete: (id: string, config?: ApiRequestConfig) => del('mailtemplates/' + id, null, config),
};
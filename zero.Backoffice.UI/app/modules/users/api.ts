import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("users/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('users/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('users', { ...config, params: { ...query } }),

  //create: (model: any, config?: ApiRequestConfig) => post('countries', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('users/' + model.id, model, config),

  //delete: (id: string, config?: ApiRequestConfig) => del('countries/' + id, null, config),

  //updatePassword: async model => await post(base + 'updatePassword', model),

  //hashPassword: async model => await post(base + 'hashPassword', model),

  //disable: async model => await post(base + 'disable', model),

  //enable: async model => await post(base + 'enable', model)
};
import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getTypes: (config?: ApiRequestConfig) => get('spaces/types', { ...config }),

  getType: (alias: string, config?: ApiRequestConfig) => get('spaces/types/' + alias, { ...config }),

  getByAlias: (alias: string, query?: ApiRequestQuery, config?: ApiRequestConfig) => get('spaces/' + alias, { ...config, params: { ...(query || {}) }}),

  getEmpty: (alias: string, config?: ApiRequestConfig) => get("spaces/" + alias + "/empty", { ...config }),

  getById: (alias: string, id: string, config?: ApiRequestConfig) => get("spaces/" + alias + "/" + id, { ...config }),

  create: (model: any, config?: ApiRequestConfig) => post('spaces', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('spaces/' + model.id, model, config),

  delete: (id: string, config?: ApiRequestConfig) => del('spaces/' + id, null, config),
};
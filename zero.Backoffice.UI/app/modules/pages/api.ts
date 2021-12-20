import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getChildren: (id: string, query: ApiRequestQuery, config?: ApiRequestConfig) => get(`pages/${id}/children`, { ...config, params: { ...query } }),

  tree: {

    getChildren: (id: string, activeId?: string, search?: string, config?: ApiRequestConfig) => get(`backoffice/pages/${id}/children`, { ...config, params: { activeId, search } }),

  }

  //getTypes: (config?: ApiRequestConfig) => get('spaces/types', { ...config }),

  //getType: (alias: string, config?: ApiRequestConfig) => get('spaces/types/' + alias, { ...config }),

  //getByAlias: (alias: string, query?: ApiRequestQuery, config?: ApiRequestConfig) => get('spaces/' + alias, { ...config, params: { ...(query || {}) }}),

  //getEmpty: (alias: string, config?: ApiRequestConfig) => get("spaces/empty", { ...config, params: { alias } }),


  //getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('countries/' + id, { ...config, params: { changeVector } }),

  //getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('countries', { ...config, params: { ...query } }),

  //create: (model: any, config?: ApiRequestConfig) => post('countries', model, config),

  //update: (model: any, config?: ApiRequestConfig) => put('countries/' + model.id, model, config),

  //delete: (id: string, config?: ApiRequestConfig) => del('countries/' + id, null, config),
};
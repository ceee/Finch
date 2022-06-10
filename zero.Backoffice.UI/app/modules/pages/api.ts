import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  tree: {
    getChildren: (id: string, activeId?: string, search?: string, config?: ApiRequestConfig) => get(`backoffice/pages/${id}/children`, { ...config, params: { activeId, search } }),
  },

  getEmpty: (flavor?: string, parentId?: string, config?: ApiRequestConfig) => get("pages/empty", { ...config, params: { flavor, parentId } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('pages/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('pages', { ...config, params: { ...query } }),

  getChildren: (id: string, query: ApiRequestQuery, config?: ApiRequestConfig) => get(`pages/${id}/children`, { ...config, params: { ...query } }),

  getAllowedFlavors: (id: string, config?: ApiRequestConfig) => get(`pages/${id}/flavors`, { ...config }),

  getDependencies: (id: string, config?: ApiRequestConfig) => get(`backoffice/pages/${id}/dependencies`, { ...config }),

  getPreviews: (ids: string[], config?: ApiRequestConfig) => get(`backoffice/pages/previews`, { ...config, params: { ids } }),

  getUrl: (id: string, config?: ApiRequestConfig) => get(`pages/${id}/url`, { ...config }),

  create: (model: any, config?: ApiRequestConfig) => post('pages', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('pages/' + model.id, model, config),

  move: (id: string, parentId: string, config?: ApiRequestConfig) => put(`pages/${id}/move/${parentId}`, {}, { ...config }),

  copy: (id: string, parentId: string, includeDescendants?: boolean, config?: ApiRequestConfig) => put(`pages/${id}/copy/${parentId}`, {}, { ...config, params: { includeDescendants: (includeDescendants || false) } }),

  sort: (ids: string[], config?: ApiRequestConfig) => put('pages/sort', ids, config),

  delete: (id: string, config?: ApiRequestConfig) => del('pages/' + id, null, config),

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
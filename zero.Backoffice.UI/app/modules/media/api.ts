import { MediaType } from 'zero/media';
import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (type: MediaType, flavor?: string, config?: ApiRequestConfig) => get("media/empty", { ...config, params: { type, flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('media/' + id, { ...config, params: { changeVector } }),

  getChildren: (id: string, query: ApiRequestQuery, config?: ApiRequestConfig) => get(`media/${id}/children`, { ...config, params: { ...query } }),

  getFolderChildren: (id: string, query: ApiRequestQuery, config?: ApiRequestConfig) => get(`media/${id}/children`, { ...config, params: { ...query, folders: true } }),

  getHierarchy: (id: string, config?: ApiRequestConfig) => get(`media/${id}/hierarchy`, { ...config }),

  bulkDelete: (ids: string[], config?: ApiRequestConfig) => del(`media/bulk/delete`, { ids }, { ...config }),

  bulkMove: (ids: string[], parentId: string, config?: ApiRequestConfig) => put(`media/bulk/move`, { parentId, ids }, { ...config }),

  bulkGetDescendants: (ids: string[], config?: ApiRequestConfig) => get(`media/bulk/descendants`, { ...config, params: { ids } }),

  //create: (model: any, config?: ApiRequestConfig) => post('countries', model, config),

  //update: (model: any, config?: ApiRequestConfig) => put('countries/' + model.id, model, config),

  //delete: (id: string, config?: ApiRequestConfig) => del('countries/' + id, config),
};
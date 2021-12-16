import { MediaType } from 'zero/media';
import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getEmpty: (type: MediaType, flavor?: string, config?: ApiRequestConfig) => get("media/empty", { ...config, params: { type, flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('media/' + id, { ...config, params: { changeVector } }),

  getChildren: (id: string, query: ApiRequestQuery, config?: ApiRequestConfig) => get(`media/${id}/children`, { ...config, params: { ...query } }),

  getHierarchy: (id: string, config?: ApiRequestConfig) => get(`media/${id}/hierarchy`, { ...config }),


  //create: (model: any, config?: ApiRequestConfig) => post('countries', model, config),

  //update: (model: any, config?: ApiRequestConfig) => put('countries/' + model.id, model, config),

  //delete: (id: string, config?: ApiRequestConfig) => del('countries/' + id, config),
};
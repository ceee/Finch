import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';


const files = {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("media/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('media/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('media', { ...config, params: { ...query } }),

  getHierarchy: (id: string, config?: ApiRequestConfig) => get(`media/${id}/hierarchy`, { ...config }),

  search: (searchQuery: string, parentId: string | null, query: ApiRequestQuery, config?: ApiRequestConfig) => get('media/search', { ...config, params: { ...query, q: searchQuery, parent: parentId } }),

  //create: (model: any, config?: ApiRequestConfig) => post('media', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('media/' + model.id, model, config),

  move: (id: string, parentId: string, config?: ApiRequestConfig) => put(`media/${id}/move/${parentId}`, {}, { ...config }),

  delete: (id: string, config?: ApiRequestConfig) => del('media/' + id, config),

  upload: async (file: File, folderId?: string, onProgress?: any) =>
  {
    var data = new FormData();
    data.append('file', file);

    if (folderId)
    {
      data.append('folderId', folderId);
    }

    return await post('media', data, {
      onUploadProgress: (progressEvent) =>
      {
        if (typeof onProgress === 'function')
        {
          var percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
          onProgress(percentCompleted);
        }
      }
    });
  }
};


const folders = {

  getEmpty: (flavor?: string, config?: ApiRequestConfig) => get("media/folders/empty", { ...config, params: { flavor } }),

  getById: (id: string, changeVector?: string, config?: ApiRequestConfig) => get('media/folders/' + id, { ...config, params: { changeVector } }),

  getByQuery: (query: ApiRequestQuery, config?: ApiRequestConfig) => get('media/folders', { ...config, params: { ...query } }),

  getChildren: (id: string, includeFiles: boolean, query: ApiRequestQuery, config?: ApiRequestConfig) => get(`media/folders/${id}/children`, { ...config, params: { ...query, files: includeFiles } }),

  getHierarchy: (id: string, config?: ApiRequestConfig) => get(`media/folders/${id}/hierarchy`, { ...config }),

  create: (model: any, config?: ApiRequestConfig) => post('media/folders', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('media/folders/' + model.id, model, config),

  move: (id: string, parentId: string, config?: ApiRequestConfig) => put(`media/folders/${id}/move/${parentId}`, {}, { ...config }),

  delete: (id: string, config?: ApiRequestConfig) => del('media/folders/' + id, config)
};


const bulk = {

  delete: (ids: string[], config?: ApiRequestConfig) => del(`media/bulk/delete`, { ids }, { ...config }),

  move: (ids: string[], parentId: string, config?: ApiRequestConfig) => put(`media/bulk/move`, { parentId, ids }, { ...config }),

  getDescendants: (ids: string[], config?: ApiRequestConfig) => get(`media/bulk/descendants`, { ...config, params: { ids } }),
};


export default {
  ...files,
  folders,
  bulk
};
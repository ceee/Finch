import { get, post, put, del } from '../helpers/request.ts';

export default {
  getById: async (id, config) => await get('countries/' + id, { ...config }),
  getEmpty: async config => await get('countries/empty', { ...config }),
  getByQuery: async (query, config) => await get('countries', { ...config, params: { query } }),
  create: async (model, config) => await post('countries', model, { ...config }),
  update: async (model, config) => await put('countries/' + model.id, model, { ...config }),
  delete: async (id, config) => await del('countries/' + id, { ...config, params: { id } })
};
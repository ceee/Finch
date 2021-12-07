import { get, post, del } from '../helpers/request.ts';

const base = 'spaces/';

export default {
  getByAlias: async alias => await get(base + 'getByAlias', { params: { alias } }),

  getAll: async () => await get(base + 'getAll'),

  getList: async (alias, query) =>
  {
    query.alias = alias;
    return await get(base + 'getList', { params: query })
  },

  getContent: async (alias, contentId) => await get(base + 'getContent', { params: { alias, contentId } }),

  save: async model => await post(base + 'save', model),

  delete: async(alias, id) => await del(base + 'delete', { params: { alias, id } })
};
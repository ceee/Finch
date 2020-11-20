import { get, post, del } from '../helpers/request.ts';

const base = 'recycleBin/';

export default {
  getByQuery: async query => await get(base + 'getByQuery', { params: query }),

  getCountByOperation: async operationId => await get(base + 'getCountByOperation', { params: { operationId } }),

  delete: async id => await del(base + 'delete', { params: { id } }),

  deleteByGroup: async group => await del(base + 'deleteByGroup', { params: { group } })
};
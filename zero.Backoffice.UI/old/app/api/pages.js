import { collection, get, post, del } from '../helpers/request.ts';

const base = 'pages/';

export default {
  ...collection(base),

  getAllowedPageTypes: async parent => await get(base + 'getAllowedPageTypes', { params: { parent } }),

  getPageType: async alias => await get(base + 'getPageType', { params: { alias } }),

  getChildren: async (parent, active, search) => await get(base + 'getChildren', { params: { parent, active, search } }),

  getUrls: async id => await get(base + 'getUrls', { params: { pageId: id } }),

  getEmpty: async (type, parent) => await get(base + 'getEmptyByType', { params: { type, parent } }),

  getRevisions: async (id, page) => await get(base + 'getRevisions', { params: { id, page } }),

  saveSorting: async ids => await post(base + 'saveSorting', ids),

  move: async (id, destinationId) => await post(base + 'move', { id, destinationId }),

  copy: async (id, destinationId, includeDescendants) => await post(base + 'copy', { id, destinationId, includeDescendants }),

  restore: async (id, includeDescendants) => await post(base + 'restore', { id, includeDescendants }),

  delete: async (id, moveToRecycleBin) => await del(base + 'delete', { params: { id, moveToRecycleBin } }),
};
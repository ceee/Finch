import { get } from '../helpers/request.ts';

const base = 'modules/';

export default {
  getModuleTypes: async (tags, pageId) => await get(base + 'getModuleTypes', { params: { tags, pageId } }),

  getModuleType: async alias => await get(base + 'getModuleType', { params: { alias } }),

  getEmpty: async alias => await get(base + 'getEmpty', { params: { alias } })
};
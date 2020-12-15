import { get, post, del } from '../helpers/request.ts';

const base = 'integrations/';

export default {
  getAll: async () => await get(base + 'getAll'),
  getEmptySettings: async alias => await get(base + 'getEmptySettings', { params: { alias } }),
  getSettingsById: async id => await get(base + 'getSettingsById', { params: { id } }),
  getSettingsByAlias: async alias => await get(base + 'getSettingsByAlias', { params: { alias } }),
  save: async model => await post(base + 'save', model),
  delete: async id => await del(base + 'delete', { params: { id } })
};
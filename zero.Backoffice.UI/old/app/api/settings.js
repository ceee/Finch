import { get } from '../helpers/request.ts';

export default {
  getAreas: async () => await get('settings/getAreas')
};
import { get } from '../helpers/request.ts';

export default {
  getConfig: async () => await get('zerovue/config')
};
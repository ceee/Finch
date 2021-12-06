import { collection, get } from '../helpers/request.ts';

export default {
  ...collection('languages/'),

  getAllCultures: async () => await get('languages/getAllCultures'),

  getSupportedCultures: async () => await get('languages/getSupportedCultures')
};
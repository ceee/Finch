import { get } from '../helpers/request.ts';

export default {
  getAll: async () => await get('sections/getAll')
};
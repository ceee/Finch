import { get } from '../helpers/request.ts';

export default {
  getById: async (id) => await get('blueprint/getById', { params: { id } })
};
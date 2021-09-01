import { get } from '../helpers/request.ts';

export default {
  query: async (query) => await get('search/query', { params: { query } })
};
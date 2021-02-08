import { get } from '../helpers/request.ts';

export default {
  getChildren: async (parent, active, search) => await get('pageTree/getChildren', { params: { parent, active, search } })
};
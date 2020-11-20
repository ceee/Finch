import { get } from '../helpers/request.ts';

export default {
  getChildren: async (parent, active) => await get('pageTree/getChildren', { params: { parent, active } })
};
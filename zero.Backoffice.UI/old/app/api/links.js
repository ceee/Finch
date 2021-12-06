import { get, post, del } from '../helpers/request.ts';

const base = 'links/';

export default {
  getPreviews: async links => await post(base + 'getPreviews', links)
};
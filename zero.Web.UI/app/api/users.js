import { collection, post } from '../helpers/request.ts';

const base = 'users/';

export default {
  ...collection(base),

  updatePassword: async model => await post(base + 'updatePassword', model),

  disable: async model => await post(base + 'disable', model),

  enable: async model => await post(base + 'enable', model)
};
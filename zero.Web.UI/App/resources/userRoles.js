import { collection, get } from '../helpers/request.ts';

export default {
  ...collection('userRoles/'),

  getAllPermissions: async () => await get('userRoles/getAllPermissions')
};
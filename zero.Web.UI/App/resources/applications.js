import { collection, get } from '../helpers/request.ts';

export default {
  ...collection('applications/'),

  getAllFeatures: async () => await get('applications/getAllFeatures')
};
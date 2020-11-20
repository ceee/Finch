import { collection, get, post } from '../helpers/request.ts';

export default {
  ...collection('mediaFolder/'),

  getHierarchy: async id => await get('mediaFolder/getHierarchy', { params: { id } }),

  getAllAsTree: async (parent, active) => await get('mediaFolder/getAllAsTree', { params: { parent, active } }),

  move: async (id, destinationId) => await post('mediaFolder/move', { id, destinationId })
};
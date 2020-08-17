import Axios from 'axios';

const base = 'pages/';

export default {

  getAllowedPageTypes(parent)
  {
    return Axios.get(base + 'getAllowedPageTypes', { params: { parent } }).then(res => Promise.resolve(res.data));
  },

  getPageType(alias)
  {
    return Axios.get(base + 'getPageType', { params: { alias } }).then(res => Promise.resolve(res.data));
  },

  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  getEmpty(type, parent)
  {
    return Axios.get(base + 'getEmpty', { params: { type, parent } }).then(res => Promise.resolve(res.data));
  },

  getRevisions(id)
  {
    return Axios.get(base + 'getRevisions', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  getAll(query)
  {
    return Axios.get(base + 'getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  save(model)
  {
    return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  saveSorting(ids)
  {
    return Axios.post(base + 'saveSorting', ids).then(res => Promise.resolve(res.data));
  },

  move(id, destinationId)
  {
    return Axios.post(base + 'move', { id, destinationId }).then(res => Promise.resolve(res.data));
  },

  copy(id, destinationId, includeDescendants)
  {
    return Axios.post(base + 'copy', { id, destinationId, includeDescendants }).then(res => Promise.resolve(res.data));
  },

  delete(id, moveToRecycleBin)
  {
    return Axios.delete(base + 'delete', { params: { id, moveToRecycleBin } }).then(res => Promise.resolve(res.data));
  }
};
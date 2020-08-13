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

  move(id, parentId)
  {
    return Axios.post(base + 'move', { id, parentId }).then(res => Promise.resolve(res.data));
  },

  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
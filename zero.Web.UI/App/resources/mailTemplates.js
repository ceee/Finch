import Axios from 'axios';

const base = 'mailTemplates/';

export default {

  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  getEmpty(id)
  {
    return Axios.get(base + 'getEmpty').then(res => Promise.resolve(res.data));
  },

  getByQuery(query)
  {
    return Axios.get(base + 'getByQuery', { params: query }).then(res => Promise.resolve(res.data));
  },

  getAll(query)
  {
    return Axios.get(base + 'getAll', query ? { params: { query } } : undefined).then(res => Promise.resolve(res.data));
  },

  getPreviews(ids)
  {
    return Axios.get(base + 'getPreviews', { params: { ids } }).then(res => Promise.resolve(res.data));
  },

  getForPicker()
  {
    return Axios.get(base + 'getForPicker').then(res => Promise.resolve(res.data));
  },

  save(model)
  {
    return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
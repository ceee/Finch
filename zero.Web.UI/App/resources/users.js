import Axios from 'axios';
import AuthApi from 'zero/services/auth';

const base = 'users/';

export default {

  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  getAll(query)
  {
    return Axios.get(base + 'getAll', { params: query }).then(res => Promise.resolve(res.data));
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
  },

  updatePassword(model)
  {
    return Axios.post(base + 'updatePassword', model).then(res => Promise.resolve(res.data));
  },

  disable(model)
  {
    return Axios.post(base + 'disable', model).then(res => Promise.resolve(res.data));
  },

  enable(model)
  {
    return Axios.post(base + 'enable', model).then(res => Promise.resolve(res.data));
  }
};
import Axios from 'axios';

export default {

  getById(id)
  {
    return Axios.get('countries/getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  getEmpty()
  {
    return Axios.get('countries/getEmpty').then(res => Promise.resolve(res.data));
  },

  getAll(query)
  {
    return Axios.get('countries/getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  getPreviews(ids)
  {
    return Axios.get('countries/getPreviews', { params: { ids } }).then(res => Promise.resolve(res.data));
  },

  getForPicker()
  {
    return Axios.get('countries/getForPicker').then(res => Promise.resolve(res.data));
  },

  save(model)
  {
    return Axios.post('countries/save', model).then(res => Promise.resolve(res.data));
  },

  delete(id)
  {
    return Axios.delete('countries/delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
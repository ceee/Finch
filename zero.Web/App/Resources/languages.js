import Axios from 'axios';

export default {

  // get language by id
  getById(id)
  {
    return Axios.get('languages/getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get all languages
  getAll()
  {
    return Axios.get('languages/getAll').then(res => Promise.resolve(res.data));
  },

  // get all available cultures
  getAllCultures()
  {
    return Axios.get('languages/getAllCultures').then(res => Promise.resolve(res.data));
  },

  // save a language
  save(model)
  {
    return Axios.post('languages/save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a language
  delete(id)
  {
    return Axios.delete('languages/delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
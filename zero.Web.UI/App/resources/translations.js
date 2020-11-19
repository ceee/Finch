import Axios from 'axios';

export default {

  // get translation by id
  getById(id)
  {
    return Axios.get('translations/getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get new translation model
  getEmpty(id)
  {
    return Axios.get('translations/getEmpty').then(res => Promise.resolve(res.data));
  },

  // get all translations
  getByQuery(query)
  {
    return Axios.get('translations/getByQuery', { params: query }).then(res => Promise.resolve(res.data));
  },

  // get all translations
  getAll(query)
  {
    return Axios.get('translations/getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  // save a translation
  save(model)
  {
    return Axios.post('translations/save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a translation
  delete(id)
  {
    return Axios.delete('translations/delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
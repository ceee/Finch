import Axios from 'axios';

const base = 'applications/';

export default {

  // get application by id
  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get new application model
  getEmpty()
  {
    return Axios.get(base + 'getEmpty').then(res => Promise.resolve(res.data));
  },

  // get all applications
  getAll(query)
  {
    return Axios.get(base + 'getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  // get all application features
  getAllFeatures()
  {
    return Axios.get(base + 'getAllFeatures').then(res => Promise.resolve(res.data));
  },

  // save an application
  save(model)
  {
    return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  // deletes an application
  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
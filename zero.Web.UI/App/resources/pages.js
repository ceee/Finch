import Axios from 'axios';

const base = 'pages/';

export default {

  // get allowed page types for parent page
  getAllowedPageTypes(parent)
  {
    return Axios.get(base + 'getAllowedPageTypes', { params: { parent } }).then(res => Promise.resolve(res.data));
  },

  // get media by id
  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get new media model
  getEmpty()
  {
    return Axios.get(base + 'getEmpty').then(res => Promise.resolve(res.data));
  },

  // get all media items
  getAll(query)
  {
    return Axios.get(base + 'getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  // save a media
  save(model)
  {
    return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a media
  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
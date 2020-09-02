import Axios from 'axios';

const base = 'mediaFolder/';

export default {

  // get media folder by id
  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get new media folder model
  getEmpty()
  {
    return Axios.get(base + 'getEmpty').then(res => Promise.resolve(res.data));
  },

  // get media folder hierarchy by id
  getHierarchy(id)
  {
    return Axios.get(base + 'getHierarchy', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get all folder with a certain parent (can be empty) as tree
  getAllAsTree(parent, active)
  {
    return Axios.get(base + 'getAllAsTree', {
      params: {
        parent: parent,
        active: active
      }
    }).then(res => Promise.resolve(res.data));
  },

  // save a media folder
  save(model)
  {
    return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a media folder
  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
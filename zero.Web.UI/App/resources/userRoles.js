import Axios from 'axios';

export default {

  // get role by id
  getById(id)
  {
    return Axios.get('userRoles/getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get all roles
  getAll()
  {
    return Axios.get('userRoles/getAll').then(res => Promise.resolve(res.data));
  },

  // get all permissions
  getAllPermissions()
  {
    return Axios.get('userRoles/getAllPermissions').then(res => Promise.resolve(res.data));
  },

  // save a role
  save(model)
  {
    return Axios.post('userRoles/save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a role
  delete(id)
  {
    return Axios.delete('userRoles/delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};
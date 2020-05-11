import Axios from 'axios';
import AuthApi from 'zero/services/auth';

export default {

  // get user by id
  getById(id)
  {
    return Axios.get('users/getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get all users
  getAll(query)
  {
    return Axios.get('users/getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  // save a user
  save(model)
  {
    return Axios.post('users/save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a user
  delete(id)
  {
    return Axios.delete('users/delete', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // change password
  updatePassword(model)
  {
    return Axios.post('users/updatePassword', model).then(res => Promise.resolve(res.data));
  },

  // disable user
  disable(model)
  {
    return Axios.post('users/disable', model).then(res => Promise.resolve(res.data));
  },

  // enable user
  enable(model)
  {
    return Axios.post('users/enable', model).then(res => Promise.resolve(res.data));
  }
};
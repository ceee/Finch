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
  }
};
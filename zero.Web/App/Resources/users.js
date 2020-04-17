import Axios from 'axios';

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
  }
};
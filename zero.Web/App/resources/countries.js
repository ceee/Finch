import Axios from 'axios';

export default {

  // get country by id
  getById(id)
  {
    return Axios.get('countries/getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get all countries
  getAll(query)
  {
    return Axios.get('countries/getAll', { params: query }).then(res => Promise.resolve(res.data));
  }
};
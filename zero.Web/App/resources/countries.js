import Axios from 'axios';

export default {

  // get all countries
  getAll(query)
  {
    return Axios.get('countries/getAll', { params: query }).then(res => Promise.resolve(res.data));
  }
};
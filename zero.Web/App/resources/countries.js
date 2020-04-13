import Axios from 'axios';

export default {

  // get all countries
  getAll()
  {
    return Axios.get('countries/getAll').then(res => Promise.resolve(res.data));
  }
};
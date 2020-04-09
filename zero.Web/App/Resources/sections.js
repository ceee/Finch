import Axios from 'axios';

export default {

  // get all sections
  getAll()
  {
    return Axios.get('sections/getAll').then(res => Promise.resolve(res.data));
  }
};
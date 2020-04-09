import Axios from 'axios';

export default {

  // get all applications
  getAll()
  {
    return Axios.get('applications/getAll').then(res => Promise.resolve(res.data));
  }
};
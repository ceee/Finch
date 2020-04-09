import Axios from 'axios';

export default {

  // get all settings areas
  getAreas()
  {
    return Axios.get('settings/getAreas').then(res => Promise.resolve(res.data));
  }
};
import Axios from 'axios';

export default {

  // get renderer by alias
  getByAlias(alias)
  {
    return Axios.get('renderer/getByAlias', { params: { alias } }).then(res => Promise.resolve(res.data));
  }
};
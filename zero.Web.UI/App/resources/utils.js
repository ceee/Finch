import Axios from 'axios';

const base = 'utils/';

export default {

  generateAlias(name)
  {
    return Axios.get(base + 'generateAlias', { params: { name } }).then(res => Promise.resolve(res.data));
  }
};
import Axios from 'axios';

export default {

  // get all list collections
  getCollections()
  {
    return Axios.get('lists/getCollections').then(res => Promise.resolve(res.data));
  },

  // get all list items in a collection
  getAll(alias, query)
  {
    query.alias = alias;

    return Axios.get('lists/getAll', { params: query }).then(res => Promise.resolve(res.data));
  }
};
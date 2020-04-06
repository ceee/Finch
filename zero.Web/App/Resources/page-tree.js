import Axios from 'axios';

export default {

  // get all pages with a certain parent (can be empty)
  getChildren(parent)
  {
    return Axios.get(zero.path + 'api/pageTree/getChildren', {
      params: {
        parent: parent
      }
    }).then(res => Promise.resolve(res.data));
  }
};
import Axios from 'axios';

export default {

  // get all pages with a certain parent (can be empty)
  getChildren(parent, active)
  {
    return Axios.get('pageTree/getChildren', {
      params: {
        parent: parent,
        active: active
      }
    }).then(res => Promise.resolve(res.data));
  }
};
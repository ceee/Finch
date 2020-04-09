import Vue from 'vue';
import { find as _find, extend as _extend } from 'underscore';

export default new Vue({

  data: () => ({
    isAuthenticated: false,
    user: null
  }),

  methods: {

    // loads the current user into the cache
    loadUser()
    {

    },

    // the cached user has been rejected by the server so we clear credentials here
    rejectUser()
    {
      this.isAuthenticated = false;
      this.user = null;
    },

    // logs the user in with the passed credentials
    login(model)
    {
      console.info(model);
      return new Promise((resolve, reject) =>
      {
        setTimeout(() =>
        {
          if (model.email && model.password)
          {
            resolve(model);
          }
          else
          {
            reject([
              {
                field: 'email',
                message: 'The email is not valid'
              },
              {
                field: 'nonexisting',
                message: 'This field does not exist'
              }
            ]);
          }
          //resolve(this.model);
        }, 1000);
      });
    },

    // logs the current user out
    logout()
    {

    }
  }
});
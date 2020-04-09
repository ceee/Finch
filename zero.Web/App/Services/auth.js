import Vue from 'vue';
import { find as _find, extend as _extend } from 'underscore';

export default new Vue({

  data: () => ({
    isAuthenticated: true,
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
    login()
    {

    },

    // logs the current user out
    logout()
    {

    }
  }
});
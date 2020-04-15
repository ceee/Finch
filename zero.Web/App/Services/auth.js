import Vue from 'vue';
import { find as _find, extend as _extend } from 'underscore';
import Axios from 'axios';

export default new Vue({

  data: () => ({
    isAuthenticated: false,
    user: {
      name: null,
      email: null
    }
  }),

  watch: {
    isAuthenticated(value)
    {
      this.$emit('authenticated', value);
    },
    user(value)
    {
      this.$emit('user', value);
    }
  },

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

    // sets the current user and isAuthenticated to true
    setUser(user)
    {
      if (!user)
      {
        this.rejectUser();
        return;
      }
      this.isAuthenticated = true;
      this.user = user;
    },

    // logs the user in with the passed credentials
    login(model)
    {
      return Axios.post('authentication/loginUser', model).then(res =>
      {
        return new Promise((resolve, reject) =>
        {
          if (res.data.success)
          {
            this.setUser(res.data.model);
            resolve(res.data.model);
          }
          else
          {
            this.rejectUser();
            reject(res.data.errors);
          }
        });
      });
    },

    // logs the current user out
    logout()
    {

    }
  }
});
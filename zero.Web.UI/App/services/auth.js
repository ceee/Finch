import Vue from 'vue';
import Overlay from 'zero/services/overlay';
import { find as _find, extend as _extend } from 'underscore';
import Axios from 'axios';
import PasswordChangeOverlay from 'zero/pages/password-change';

export default new Vue({

  data: () => ({
    isAuthenticated: false,
    rejectReason: null,
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
      Axios.get('authentication/getUser').then(res =>
      {
        this.isAuthenticated = res.data.success && res.data.model;

        if (res.data.success)
        {
          this.user = res.data.model;
        }
      });
    },

    // the cached user has been rejected by the server so we clear credentials here
    rejectUser(reason)
    {
      this.rejectReason = reason;
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
            resolve(res.data);
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
      let promise = Axios.post('authentication/logoutUser');
      this.rejectUser("@login.rejectReasons.logout");
      return promise;
    },

    // open overlay to update password
    openPasswordOverlay()
    {
      return Overlay.open({
        title: '@changepasswordoverlay.title',
        closeLabel: '@ui.close',
        confirmLabel: '@changepasswordoverlay.confirm',
        component: PasswordChangeOverlay
      });
    }
  }
});
<template>
  <div class="app">
    <template v-if="isAuthenticated">
      <app-overlays />
      <app-navigation />
      <div class="app-main">
        <router-view></router-view>
      </div>
    </template>
    <app-auth v-else />
  </div>
</template>


<script>
  import '../Sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'
  import AppAuth from 'zeropages/login.vue'
  import AppOverlays from 'zerocomponents/Overlays/overlay-holder.vue'
  import Router from '../router.js'
  import Auth from 'zeroservices/auth.js';
  import 'zero/axios.config.js'

  export default {
    name: 'app',

    router: Router,

    components: { AppNavigation, AppOverlays, AppAuth },

    data: () => ({
      isLoading: false,
      isAuthenticated: Auth.isAuthenticated
    }),

    mounted ()
    {
      //AuthApi.getUser(res =>
      //{
      //  Store.state.isAuthenticated = res.isAuthenticated;
      //  Store.state.user = res.user;
      //  Store.state.userMap = res.userMap;
      //  this.isLoading = false;
      //});
    }
  }

</script>
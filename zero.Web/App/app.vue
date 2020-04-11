<template>
  <div class="app">
    <template v-if="isAuthenticated">
      <app-navigation />
      <div class="app-main">
        <router-view></router-view>
      </div>
      <app-overlays />
    </template>
    <app-login v-else />
  </div>
</template>


<script>
  import '../Sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'
  import AppLogin from 'zeropages/login.vue'
  import AppOverlays from 'zerocomponents/Overlays/overlay-holder.vue'
  import Router from 'zero/app.router.js'
  import AuthApi from 'zeroservices/auth.js'
  import 'zero/axios.config.js'

  export default {
    name: 'app',

    router: Router,

    components: { AppNavigation, AppOverlays, AppLogin },

    data: () => ({
      isAuthenticated: true
    }),

    created()
    {
      AuthApi.$on('authenticated', isAuthenticated =>
      {
        this.isAuthenticated = isAuthenticated;
      });
    }
  }

</script>
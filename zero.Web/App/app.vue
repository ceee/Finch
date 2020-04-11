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
  import AppLogin from 'zero/pages/login/login.vue'
  import AppOverlays from 'zero/components/overlays/overlay-holder.vue'
  import Router from 'zero/router.config.js'
  import AuthApi from 'zero/services/auth.js'
  import 'zero/vue.config.js'
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
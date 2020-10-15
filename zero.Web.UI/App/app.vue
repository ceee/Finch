<template>
  <div class="app" :class="getClassList()">
    <template v-if="isAuthenticated">
      <app-navigation />
      <div class="app-main">
        <router-view></router-view>
      </div>
      <app-overlays />
      <app-notifications />
    </template>
    <app-login v-else />
  </div>
</template>


<script>
  import '../sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'
  import AppLogin from 'zero/pages/login/login.vue'
  import AppOverlays from 'zero/components/overlays/overlay-holder.vue'
  import AppNotifications from 'zero/components/notifications/notification-holder.vue'
  import Router from 'zero/router.config.js'
  import AuthApi from 'zero/services/auth.js'
  import 'zero/vue.config.js'
  import 'zero/axios.config.js'
  import 'zero/zero.config.js'

  export default {
    name: 'app',

    router: Router,

    components: { AppNavigation, AppOverlays, AppLogin, AppNotifications },

    data: () => ({
      isAuthenticated: false
    }),

    created()
    {
      AuthApi.$on('authenticated', isAuthenticated =>
      {
        this.isAuthenticated = isAuthenticated;
      });

      AuthApi.setUser(zero.user);
    },


    methods: {

      getClassList()
      {
        return {
          'is-preview': this.$route.name === 'preview'
        };
      }
    }
  }

</script>
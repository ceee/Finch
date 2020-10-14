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
  import './sass/app.scss'
  import AppNavigation from './navigation.vue'
  import AppLogin from './pages/login/login.vue'
  import AppOverlays from './components/overlays/overlay-holder.vue'
  import AppNotifications from './components/notifications/notification-holder.vue'
  import AuthApi from './services/auth.js'
  import EventHub from './services/eventhub.js'
  import './config/zero.config.js'
  import './config/axios.config.js'

  export default {
    name: 'app',

    components: { AppNavigation, AppOverlays, AppLogin, AppNotifications },

    data: () => ({
      isAuthenticated: false
    }),

    created()
    {
      EventHub.on('authenticated', isAuthenticated =>
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
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
  import AppNavigation from '@zero/navigation.vue'
  import AppLogin from '@zero/pages/login/login.vue'
  import AppOverlays from '@zero/components/overlays/overlay-holder.vue'
  import AppNotifications from '@zero/components/notifications/notification-holder.vue'
  import AuthApi from '@zero/services/auth.js'
  import EventHub from '@zero/services/eventhub.js'
  import '@zero/config/zero.config.js'
  import '@zero/config/axios.config.js'

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
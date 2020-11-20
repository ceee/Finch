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
  import EventHub from 'zero/helpers/eventhub.js';
  import AuthApi from 'zero/helpers/auth.js'

  export default {
    name: 'app',

    components: { AppNavigation, AppOverlays, AppLogin, AppNotifications },

    data: () => ({
      isAuthenticated: false
    }),

    created()
    {
      AuthApi.setUser(__zero.user);

      AuthApi.$on('authenticated', isAuthenticated =>
      {
        this.isAuthenticated = isAuthenticated;
      });
    },


    methods: {

      getClassList()
      {
        return {
          'is-preview': false //this.$route.name === 'preview'
        };
      }
    }
  }

</script>
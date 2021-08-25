<template>
  <div class="app" :class="getClassList()" :key="appKey">
    <template v-if="isAuthenticated">
      <app-bar />
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
  import '../sass/sass.js'
  import AppNavigation from 'zero/navigation.vue'
  import AppBar from 'zero/bar.vue'
  import AppLogin from 'zero/pages/login/login.vue'
  import AppOverlays from 'zero/components/overlays/overlay-holder.vue'
  import AppNotifications from 'zero/components/notifications/notification-holder.vue'
  import EventHub from 'zero/helpers/eventhub.js';
  import AuthApi from 'zero/helpers/auth.js'
  import ConfigApi from 'zero/api/config.js';


  export default {
    name: 'app',

    components: { AppNavigation, AppBar, AppOverlays, AppLogin, AppNotifications },

    data: () => ({
      isAuthenticated: false,
      keyIndex: 0
    }),

    computed: {
      appKey()
      {
        return 'appkey-' + this.keyIndex;
      }
    },

    created()
    {
      AuthApi.setUser(__zero.user);

      AuthApi.$on('authenticated', isAuthenticated =>
      {
        this.isAuthenticated = isAuthenticated;
      });

      AuthApi.$on('appswitch', data =>
      {
        this.rerender();
        //this.isAuthenticated = isAuthenticated;
      });

      AuthApi.$on('apprebuild', data =>
      {
        this.rerender();
      });
    },


    methods: {

      getClassList()
      {
        return {
          'is-preview': false //this.$route.name === 'preview'
        };
      },

      rerender()
      {
        ConfigApi.getConfig().then(res => {
          window.__zero = res;
          window.zero = res;
          AuthApi.setUser(res.user);
          this.keyIndex += 1;
        });
      }
    }
  }

</script>
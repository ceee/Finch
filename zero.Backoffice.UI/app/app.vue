<template>
  <div class="app" v-if="!loading" :class="{ 'is-preview': preview}">
    <app-login v-if="!authenticated" />
    <template v-if="authenticated && !preview">
      <app-navigation />
      <div class="app-main">
        <router-view></router-view>
      </div>
      <app-overlays />
      <app-notifications />
    </template>
    <template v-if="authenticated && preview">
      <router-view></router-view>
    </template>
  </div>
  <div class="app-loading" v-else>
    <ui-loading />
  </div>
</template>


<script lang="ts">
  import './styles/styles';
  import AppLogin from './account/login.vue';
  import AppNavigation from './ui/app-navigation.vue';
  import AppNotifications from './ui/app-notifications.vue';
  import AppOverlays from './ui/app-overlays.vue';
  import { defineComponent } from 'vue';
  import startup from './startup';
  import { AccountUser } from 'zero/account';

  export default defineComponent({
    name: 'app',

    components: { AppLogin, AppNavigation, AppNotifications, AppOverlays },

    data: () => ({
      loading: true,
      authenticated: false
    }),

    computed: {
      preview()
      {
        return this.$route.name === 'preview';
      }
    },

    mounted()
    {
      this.startup();

      this.zero.events.on('zero.authenticate', this.onAuthentication);
      this.zero.events.on('zero.rejectauth', () => this.authenticated = false);
    },


    methods: {

      async startup()
      {
        const result = await startup(this.zero);
        this.authenticated = result.authenticated;
        this.loading = false;
      },


      async onAuthentication(user?: AccountUser)
      {
        await this.startup();
      }
    }
  });

</script>

<style lang="scss">
  .app-loading
  {
    display: flex;
    height: 100vh;
    width: 100vw;
    align-items: center;
    justify-content: center;
  }
</style>
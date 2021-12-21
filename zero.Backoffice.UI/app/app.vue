<template>
  <div class="app" v-if="initialized">
    <app-login v-if="!accountStore.isAuthenticated" />
    <template v-else>
      <app-navigation />
      <div class="app-main">
        <router-view></router-view>
      </div>
      <app-overlays />
      <app-notifications />
    </template>
  </div>
</template>


<script lang="ts">
  import './styles/styles';
  import AppLogin from './account/login.vue';
  import AppNavigation from './ui/app-navigation.vue';
  import AppNotifications from './ui/app-notifications.vue';
  import AppOverlays from './ui/app-overlays.vue';
  import { useAccountStore } from './account/store';
  import { useUiStore } from './ui/store';
  import { useTranslationStore } from './stores/translations';
  import { useAppStore } from './modules/applications/store';
  import accountApi from './account/api';
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'app',

    components: { AppLogin, AppNavigation, AppNotifications, AppOverlays },

    data: () => ({
      initialized: false,
      accountStore: null
    }),

    async mounted()
    {
      this.accountStore = useAccountStore();

      let userResponse = await accountApi.getUser();

      if (userResponse.success)
      {
        this.accountStore.user = userResponse.data;
        await useAppStore().setup();
        await useUiStore().setup();
      }
      else
      {
        await useUiStore().setup(true);
      }

      await useTranslationStore().setup();

      this.initialized = true;
    },


  });

</script>
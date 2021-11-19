<template>
  <div class="app-bar theme-dark">
    <h1 class="app-nav-headline">
      <img src="/Assets/zero-2-light.png" class="show-light" v-localize:alt="'@zero.name'" />
      <img src="/Assets/zero-2.png" class="show-dark" v-localize:alt="'@zero.name'" />
    </h1>

    <ui-dropdown v-if="applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="light block" :label="currentApplication.name" caret="down" />
      </template>
      <ui-dropdown-button v-for="app in applications" :value="app" :key="app.id" :label="app.name" :selected="app.id === appId" @click="applicationChanged" :prevent="true" />
      <ui-dropdown-separator />
      <ui-dropdown-button label="Add new application" icon="fth-plus" @click="addApplication" />
      <ui-dropdown-button label="Manage apps" icon="fth-edit-2" @click="manageApplications" />
    </ui-dropdown>

    <div></div>

    <footer class="app-nav-account" v-if="user">     
      <ui-dropdown align="right top">
        <template v-slot:button>
          <button type="button" class="app-nav-account-button">
            <img class="-image" v-if="userAvatar" :src="userAvatar" :alt="user.name" />
            <span class="-image" v-if="!userAvatar"><i class="fth-user"></i></span>
            <p class="-text"><strong>{{user.name}}</strong></p>
            <ui-icon symbol="fth-chevron-down" class="-arrow" />
          </button>
        </template>
        <ui-dropdown-button label="Edit" icon="fth-edit-2" @click="editUser" />
        <ui-dropdown-button label="Change password" icon="fth-lock" @click="changePassword" />
        <ui-dropdown-button label="Logout" icon="fth-log-out" @click="logout" />
      </ui-dropdown>
    </footer>
  </div>
</template>


<script>
  import { map as _map, find as _find } from 'underscore';
  import AuthApi from 'zero/helpers/auth.js'
  import MediaApi from 'zero/api/media.js'


  export default {
    name: 'app-bar',

    data: () => ({
      appId: zero.appId,
      applications: zero.applications,
      user: null,
      userAvatar: null,
      currentApplication: null
    }),


    created()
    {
      this.currentApplication = _find(this.applications, x => x.id === zero.appId);
      this.buildUser(AuthApi.user);

      AuthApi.$on('user', user =>
      {
        this.buildUser(user);
      });
    },


    methods: {

      buildUser(user)
      {
        this.user = user;

        if (user && user.avatarId)
        {
          this.userAvatar = MediaApi.getImageSource(user.avatarId, false, true);
        }
        else
        {
          this.userAvatar = null;
        }
      },

      editUser(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: zero.alias.settings.users + '-edit',
          params: { id: this.user.id }
        });
      },

      changePassword(item, opts)
      {
        AuthApi.openPasswordOverlay();
        opts.hide();
      },

      logout(item, opts)
      {
        AuthApi.logout();
        opts.hide();
      },

      addApplication(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: zero.alias.settings.applications + '-create'
        });
      },

      manageApplications(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: zero.alias.settings.applications
        });
      },

      applicationChanged(item, opts)
      {
        opts.loading(true);

        AuthApi.switchApp(item.id).then(success =>
        {
          opts.loading(false);
          //opts.hide();
        });
      }
    }
  }

</script>

<style lang="scss">
  .app-bar
  {
    grid-column: span 2/auto;
    background: var(--color-bg-shade-3);
    height: 70px;
    display: grid;
    align-items: center;
    grid-template-columns: auto auto 1fr auto;
    padding: 0 24px;
  }
</style>
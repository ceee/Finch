<template>
  <div class="app-nav theme-light">

    <h1 class="app-nav-headline">
      <img src="/Assets/zero-2-light.png" v-localize:alt="'@zero.name'" />
    </h1>

    <ui-dropdown v-if="applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="action block" :label="currentApplication.name" caret="down" />
      </template>
      <ui-dropdown-button v-for="app in applications" :value="app" :key="app.id" :label="app.name" :selected="app.id === appId" @click="applicationChanged" />
      <ui-dropdown-separator />
      <ui-dropdown-button label="Add new application" icon="fth-plus" @click="addApplication" />
      <ui-dropdown-button label="Manage apps" icon="fth-edit-2" @click="manageApplications" />
    </ui-dropdown>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="section.url" class="app-nav-item" :alias="section.alias" :class="{ 'has-children': hasChildren(section) }">
          <i class="app-nav-item-icon" :class="section.icon"></i>
          {{section.name | localize}}
          <i v-if="hasChildren(section)" class="app-nav-item-arrow fth-chevron-down"></i>
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) === 0">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="child.url" class="app-nav-child">
              {{child.name | localize}}
            </router-link>
          </div>
        </transition>
      </template>
    </nav>

    <footer class="app-nav-account" v-if="user">     
      <ui-dropdown align="left bottom">
        <template v-slot:button>
          <button type="button" class="app-nav-account-button">
            <img class="-image" v-if="userAvatar" :src="userAvatar" :alt="user.name" />
            <span class="-image" v-if="!userAvatar"><i class="fth-user"></i></span>
            <p class="-text"><strong>{{user.name}}</strong></p>
            <i class="-arrow fth-chevron-down"></i>
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
  import AuthApi from 'zero/services/auth.js'
  import MediaApi from 'zero/resources/media.js'

  export default {
    name: 'app-navigation',

    data: () => ({
      appId: zero.appId,
      applications: zero.applications,
      sections: zero.sections,
      user: null,
      userAvatar: null
    }),


    computed: {
      currentApplication()
      {
        return _find(this.applications, x => x.id === zero.appId);
      }
    },


    created()
    {
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
          this.userAvatar = MediaApi.getImageSource(user.avatarId);
        }
        else
        {
          this.userAvatar = null;
        }
      },

      hasChildren(section)
      {
        return section.children && section.children.length > 0;
      },

      editUser(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-edit',
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
          name: zero.alias.sections.settings + '-' + zero.alias.settings.applications + '-create'
        });
      },

      manageApplications(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: zero.alias.sections.settings + '-' + zero.alias.settings.applications
        });
      },

      applicationChanged(item, opts)
      {
        opts.loading(true);

        AuthApi.switchApp(item.id).then(success =>
        {
          opts.loading(false);
          opts.hide();

          if (success)
          {
            location.reload();
          }
        });
      }

    }
  }

</script>
<template>
  <div class="app-nav">

    <h1 class="app-nav-headline">
      <img src="/Assets/zero-2-light.png" v-localize:alt="'@zero.name'" />
    </h1>

    <ui-dropdown v-if="applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="light block" :label="currentApplication.name" caret="down" />
      </template>
      <ui-dropdown-button v-for="app in applications" :value="app" :key="app.id" :label="app.name" :selected="app.id === appId" @click="applicationChanged" />
      <ui-dropdown-separator />
      <ui-dropdown-button label="Add new application" icon="fth-plus" @click="addApplication" />
      <ui-dropdown-button label="Manage apps" icon="fth-edit-2" @click="manageApplications" />
    </ui-dropdown>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="section.url" class="app-nav-item" :alias="section.alias" :class="{ 'has-children': hasChildren(section) }">
          <ui-icon :icon="section.icon" class="app-nav-item-icon" />
          {{section.name | localize}}
          <ui-icon v-if="hasChildren(section)" icon="chevron-down" class="app-nav-item-arrow" />
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) === 0">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="child.url" class="app-nav-child">
              {{child.name | localize}}
            </router-link>
          </div>
        </transition>
      </template>
      <!-- // TODO this is only for development -->
      <!--<button type="button" class="app-nav-item" @click="$refs.iconpicker.pick()">
        <i class="app-nav-item-icon fth-droplet"></i> Icons
      </button>
      <icon-picker ref="iconpicker" :output="false" />-->
    </nav>

    <footer class="app-nav-account" v-if="user">     
      <ui-dropdown align="left bottom">
        <template v-slot:button>
          <button type="button" class="app-nav-account-button">
            <img class="-image" v-if="userAvatar" :src="userAvatar" :alt="user.name" />
            <span class="-image" v-if="!userAvatar"><i class="fth-user"></i></span>
            <p class="-text"><strong>{{user.name}}</strong></p>
            <ui-icon icon="chevron-down" class="-arrow" />
          </button>
        </template>
        <ui-dropdown-button label="Edit" icon="edit-2" @click="editUser" />
        <ui-dropdown-button label="Change password" icon="lock" @click="changePassword" />
        <ui-dropdown-button label="Logout" icon="log-out" @click="logout" />
      </ui-dropdown>
    </footer>

  </div>
</template>


<script>
  import { map as _map, find as _find } from 'underscore';
  import AuthApi from 'zero/helpers/auth.js'
  import MediaApi from 'zero/api/media.js'
  import IconPicker from 'zero/components/pickers/iconPicker/iconpicker.vue';

  export default {
    name: 'app-navigation',

    data: () => ({
      appId: zero.appId,
      applications: zero.applications,
      sections: zero.sections,
      user: null,
      userAvatar: null
    }),


    components: { IconPicker },


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
          this.userAvatar = MediaApi.getImageSource(user.avatarId, false, true);
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
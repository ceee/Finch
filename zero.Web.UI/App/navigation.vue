<template>
  <div class="app-nav" :class="{'is-compact': compact }">

    <div class="app-nav-apps theme-light">
      <ui-header-bar class="ui-tree-header" title="Applications" :back-button="false" />
      <button v-for="app in applications" :key="app.id" type="button" @click="applicationChanged(app)" class="app-nav-app" :class="{ 'is-active': app.id == appId }">
        <img :src="app.image" class="app-nav-app-icon" :alt="app.name" />
        <ui-localize :value="app.name" />
        <ui-icon v-if="app.id == appId" symbol="fth-check" class="app-nav-app-selected" />
      </button>
    </div>

    <div class="app-nav-boxed">

      <h1 class="app-nav-headline">
        <span class="app-nav-logo-circle"></span>
        <img src="/Assets/zero.svg" class="show-light" v-localize:alt="'@zero.name'" />
        <img src="/Assets/zero-dark.svg" class="show-dark" v-localize:alt="'@zero.name'" /> 
      </h1>
      
      <ui-button icon="fth-search" :stroke="2.5" type="blank" class="app-nav-search" @click="openSearch" /> 

    </div>


    <ui-dropdown v-if="applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="light block" :label="currentApplication.name" caret="right" />
      </template>
      <button v-for="app in applications" :key="app.id" type="button" @click="applicationChanged(app)" class="ui-dropdown-button has-icon" :class="{ 'is-active': app.id == appId }">
        <img :src="app.image" class="ui-dropdown-button-icon" :alt="app.name" />
        <ui-localize :value="app.name" />
        <ui-icon v-if="app.id == appId" symbol="check" class="ui-dropdown-button-selected" />
      </button>
      <!--<ui-dropdown-button v-for="app in applications" :value="app" :key="app.id" :label="app.name" :selected="app.id === appId" @click="applicationChanged" :prevent="true" />-->
      <ui-dropdown-separator />
      <ui-dropdown-button label="Add new application" icon="fth-plus" @click="addApplication" />
      <ui-dropdown-button label="Manage apps" icon="fth-edit-2" @click="manageApplications" />
    </ui-dropdown>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="section.url" class="app-nav-item" :alias="section.alias" :class="{ 'has-children': hasChildren(section) }">
          <ui-icon :symbol="section.icon" class="app-nav-item-icon" :size="18" />
          <span class="app-nav-item-text" v-localize="section.name"></span>
          <ui-icon v-if="hasChildren(section)" symbol="fth-chevron-down" class="app-nav-item-arrow" />
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) === 0">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="child.url" class="app-nav-child">
              <span class="app-nav-child-text" v-localize="child.name"></span>
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

    <ui-dropdown class="app-nav-account" v-if="user" align="left bottom">
      <template v-slot:button>
        <button type="button" class="app-nav-account-button">
          <img class="-image" v-if="userAvatar" :src="userAvatar" :alt="user.name" />
          <span class="-image" v-if="!userAvatar"><i class="fth-user"></i></span>
          <p class="-text"><strong>{{user.name}}</strong></p>
          <ui-icon symbol="fth-more-horizontal" class="-arrow" /> 
        </button>
      </template>
      <ui-dropdown-button label="Edit" icon="fth-edit-2" @click="editUser" />
      <ui-dropdown-button label="Change password" icon="fth-lock" @click="changePassword" />
      <!--<ui-dropdown-button label="Toggle sidebar" icon="fth-minimize-2" @click="toggleSidebar" />-->
      <ui-dropdown-button label="Dark theme" v-if="!darkTheme" icon="fth-moon" @click="toggleDarkTheme" />
      <ui-dropdown-button label="Light theme" v-if="darkTheme" icon="fth-sun" @click="toggleDarkTheme" />
      <ui-dropdown-button label="Logout" icon="fth-log-out" @click="logout" />
    </ui-dropdown>

  </div>
</template>


<script>
  import { map as _map, find as _find } from 'underscore';
  import AuthApi from 'zero/helpers/auth.js'
  import MediaApi from 'zero/api/media.js'
  import IconPicker from 'zero/components/pickers/iconPicker/iconpicker.vue';
  import EventHub from 'zero/helpers/eventhub.js'

  const compactCacheKey = 'zero.navigation.compact';
  const themeCacheKey = 'zero.theme';

  export default {
    name: 'app-navigation',

    data: () => ({
      appId: zero.appId,
      applications: zero.applications,
      sections: zero.sections,
      user: null,
      userAvatar: null,
      compact: false,
      darkTheme: false,
      currentApplication: null
    }),


    components: { IconPicker },


    created()
    {
      this.currentApplication = _find(this.applications, x => x.id === zero.appId);
      this.compact = localStorage.getItem(compactCacheKey) === 'true';
      this.darkTheme = localStorage.getItem(themeCacheKey) === 'dark';
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
        //opts.loading(true);

        AuthApi.switchApp(item.id).then(success =>
        {
          //opts.loading(false);
          //opts.hide();
        });
      },

      toggleSidebar()
      {
        this.compact = !this.compact;
        localStorage.setItem(compactCacheKey, this.compact.toString());
      },

      toggleDarkTheme()
      {
        this.darkTheme = !this.darkTheme;
        EventHub.$emit('app.theme', this.darkTheme ? 'dark' : 'light');
        localStorage.setItem(themeCacheKey, this.darkTheme ? 'dark' : 'light');
        document.body.classList.toggle('theme-light', !this.darkTheme);
        document.body.classList.toggle('theme-dark', this.darkTheme);
      },

      openSearch()
      {
        EventHub.$emit('app.search.open');
      }
    }
  }

</script>
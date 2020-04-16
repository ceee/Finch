<template>
  <div class="app-nav theme-dark">

    <h1 class="app-nav-headline" v-localize="'@zero.name'">zero</h1>

    <ui-dropdown v-if="applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="action block" :label="applications[0].name" caret="down" />
      </template>
      <ui-dropdown-list :items="applicationItems" :action="applicationChanged" />
    </ui-dropdown>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="section.url" class="app-nav-item" :alias="section.alias" :class="{ 'has-children': hasChildren(section) }">
          <i class="app-nav-item-icon" :class="section.icon" :style="{ color: false && section.color ? section.color : null }"></i>
          {{section.name | localize}}
          <i v-if="hasChildren(section)" class="app-nav-item-arrow fth-chevron-down"></i>
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) > -1">
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
            <img class="-image" src="https://fifty.brothers.studio/Media/Avatars/tobi.jpg" alt="Tobi" />
            <p class="-text"><strong>{{user.name}}</strong><br>Admin</p>
            <i class="-arrow fth-chevron-down"></i>
          </button>
        </template>
        <ui-dropdown-list :items="userActions" />
      </ui-dropdown>
    </footer>

  </div>
</template>


<script>
  import { map as _map } from 'underscore';
  import AuthApi from 'zero/services/auth.js'

  export default {
    name: 'app-navigation',

    data: () => ({
      applications: zero.applications,
      applicationItems: [],
      sections: zero.sections,
      user: null,
      userActions: []
    }),

    created()
    {
      this.user = AuthApi.user;
      AuthApi.$on('user', user =>
      {
        this.user = user;
      });

      this.userActions.push({
        name: 'Edit',
        icon: 'fth-edit-2'
      });
      this.userActions.push({
        name: 'Change password',
        icon: 'fth-lock'
      });
      this.userActions.push({
        name: 'Logout',
        icon: 'fth-log-out',
        action()
        {
          AuthApi.logout();
        }
      });
    },

    mounted ()
    {
      this.applicationItems = _map(this.applications, item =>
      {
        return {
          application: item,
          active: item.name === "Brothers", // TODO correct active application
          name: item.name
        };
      });

      this.applicationItems.push({
        type: 'separator'
      });

      this.applicationItems.push({
        name: 'Add new application...',
        icon: 'fth-plus',
        action(item, dropdown)
        {
          console.info('add');
        }
      });
    },


    methods: {

      hasChildren(section)
      {
        return section.children && section.children.length > 0;
      },

      applicationChanged(item, dropdown)
      {
        console.info('change');
      }

    }
  }

</script>
<template>
  <div class="app-nav">

    <h1 class="app-nav-headline" v-localize="'@zero.name'">zero</h1>

    <ui-dropdown v-if="applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="action block" :label="applications[0].name" caret="down" />
      </template>
      <ui-dropdown-list :items="applicationItems" :action="applicationChanged" />
    </ui-dropdown>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="getLink(section)" class="app-nav-item" :class="{ 'has-children': hasChildren(section) }">
          <i class="app-nav-item-icon" :class="section.icon" :style="{ color: false && section.color ? section.color : null }"></i>
          {{section.name | localize}}
          <i v-if="hasChildren(section)" class="app-nav-item-arrow fth-chevron-down"></i>
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) > -1">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="getLink(section, child)" class="app-nav-child">
              {{child.name | localize}}
            </router-link>
          </div>
        </transition>
      </template>
    </nav>

    <footer class="app-nav-account">
      <button type="button" class="app-nav-account-button">
        <img class="-image" src="https://fifty.brothers.studio/Media/Avatars/tobi.jpg" alt="Tobi" />
        <p class="-text"><strong>Tobias Klika</strong><br>Admin</p>
        <i class="-arrow fth-chevron-down"></i>
      </button>
    </footer>

  </div>
</template>


<script>
  import ApplicationsApi from 'zeroresources/applications.js'
  import SectionsApi from 'zeroresources/sections.js'
  import { map as _map } from 'underscore';

  export default {
    name: 'app-navigation',

    data: () => ({
      applications: [],
      applicationItems: [],
      sections: []
    }),

    mounted ()
    {
      //console.info(this.$router.history.current.path);

      SectionsApi.getAll().then(items =>
      {
        this.sections = items;
      });
      ApplicationsApi.getAll().then(items =>
      {
        this.applications = items;

        this.applicationItems = _map(items, item =>
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
      });
    },


    methods: {

      hasChildren(section)
      {
        return section.children && section.children.length > 0;
      },

      getName(section)
      {
        return section.alias.charAt(0).toUpperCase() + section.alias.slice(1);
      },

      getLink(section, child)
      {
        //if (section.alias === "dashboard" && !child)
        //{
        //  return '/';
        //}

        if (!child)
        {
          return '/' + section.alias;
        }

        return '/' + section.alias + '/' + child.alias;
      },

      applicationChanged(item, dropdown)
      {
        console.info('change');
      }

    }
  }

</script>
<template>
  <div class="app-nav">

    <h1 class="app-nav-headline">zero</h1>

    <div class="app-nav-switch">
      <ui-button v-if="applications.length > 0" type="action block" :label="applications[0].name" caret="down" />
    </div>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="getLink(section)" class="app-nav-item" :class="{ 'has-children': hasChildren(section) }">
          <i class="app-nav-item-icon" :class="section.icon" :style="{ color: section.color ? section.color : null }"></i>
          {{getName(section)}}
          <i v-if="hasChildren(section)" class="app-nav-item-arrow fth-chevron-down"></i>
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) > -1">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="getLink(section, child)" class="app-nav-child">
              {{child.name}}
            </router-link>
          </div>
        </transition>
      </template>
    </nav>

  </div>
</template>


<script>
  import ApplicationsApi from 'zeroresources/applications.js'
  import SectionsApi from 'zeroresources/sections.js'
  import UiButton from 'zerocomponents/buttons/button.vue'

  export default {
    name: 'app-navigation',

    components: { UiButton },

    data: () => ({
      applications: [],
      sections: []
    }),

    mounted ()
    {
      SectionsApi.getAll().then(items =>
      {
        this.sections = items;
      });
      ApplicationsApi.getAll().then(items =>
      {
        this.applications = items;
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
      }

    }
  }

</script>
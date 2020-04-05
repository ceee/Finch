<template>
  <div class="app-nav">

    <h1 class="app-nav-headline">zero</h1>

    <div class="app-nav-switch">
      <ui-button v-if="applications.length > 0" type="outline block" :label="applications[0].name" caret="down" />
    </div>

    <nav>
      <template v-for="section in sections">
        <a v-on:click.prevent="go(section)" href="#" class="app-nav-item" :class="{ 'is-active': section.alias === activeSection, 'has-children': hasChildren(section) }">
          <i class="app-nav-item-icon" :class="section.icon" :style="{ color: section.color ? section.color : null }"></i>
          {{getName(section)}}
          <i v-if="hasChildren(section)" class="fth-chevron-down"></i>
        </a>
        <div class="app-nav-children" v-if="hasChildren(section)">
          <a v-for="child in section.children" v-on:click.prevent="go(child)" href="#" class="app-nav-child">
            {{child.name}}
          </a>
        </div>
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
      activeSection: null,
      applications: [],
      sections: []
    }),

    mounted ()
    {
      SectionsApi.getAll().then(items =>
      {
        this.sections = items;
        this.activeSection = 'commerce';
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

      go(section)
      {
        this.activeSection = section.alias;
      }

    }
  }

</script>
<template>
  <div class="app-nav">

    <h1 class="app-nav-headline">zero</h1>

    <div class="app-nav-switch">
      <ui-button type="outline block" label="brothers" caret="down" />
    </div>

    <nav>
      <a v-for="section in sections" v-on:click.prevent="go(section)" href="#" class="app-nav-item" :class="{ 'is-active': section.alias === active }">
        <i class="app-nav-item-icon" :class="section.icon" :style="{ color: section.color ? section.color : null }"></i>
        {{getName(section)}}
      </a>
    </nav>

  </div>
</template>


<script>
  import SectionsApi from 'zeroresources/sections.js'
  import UiButton from 'zerocomponents/buttons/button.vue'

  export default {
    name: 'app-navigation',

    components: { UiButton },

    data: () => ({
      active: null,
      sections: []
    }),

    mounted ()
    {
      SectionsApi.getAll().then(items =>
      {
        this.sections = items;
        this.active = 'pages';
      });
    },

    methods: {

      getName(section)
      {
        return section.alias.charAt(0).toUpperCase() + section.alias.slice(1);
      },

      go(section)
      {
        this.active = section.alias;
      }

    }
  }

</script>
<template>
  <div class="app-nav">

    <h1 class="app-nav-headline">zero</h1>

    <div class="app-nav-switch">
      <ui-button type="outline block" label="brothers" caret="down" />
    </div>

    <nav v-for="section in sections">
      <a href="#" class="app-nav-item">
        <i class="app-nav-item-icon" :class="section.icon" :style="{ color: section.color ? section.color : 'var(--color-fg-reverse-mid)' }"></i>
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
      sections: []
    }),

    mounted ()
    {
      SectionsApi.getAll().then(items =>
      {
        this.sections = items;
      });
    },

    methods: {

      getName(section)
      {
        return section.alias.charAt(0).toUpperCase() + section.alias.slice(1);
      }

    }
  }

</script>
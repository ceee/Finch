<template>
  <div class="space-editor">
    <ui-header-bar :title="space.name" title-empty="Space">
      
    </ui-header-bar>
    <ui-editor />
  </div>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';
  import UiEditor from 'zero/editor/editor';

  export default {
    props: ['config', 'space'],

    components: { UiEditor },

    data: () => ({
      
    }),

    watch: {
      '$route': 'load'
    },

    created()
    {
      this.load();
    },


    methods: {

      load()
      {
        
      }

    }
  }
</script>
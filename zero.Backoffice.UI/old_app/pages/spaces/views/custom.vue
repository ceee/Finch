<template>
  <div class="space-editor">
    <ui-header-bar :title="space.name" title-empty="Space">
      
    </ui-header-bar>
    <div class="ui-blank-box">
      custom
    </div>
  </div>
</template>


<script>
  import SpacesApi from 'zero/api/spaces.js';

  export default {
    props: [ 'alias' ],

    data: () => ({
      space: {},
      tableConfig: {}
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
        SpacesApi.getByAlias(this.alias).then(response =>
        {
          this.space = response;
        });
      }

    }
  }
</script>
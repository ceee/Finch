<template>
  <div class="space">
    <component v-if="loaded && component" :is="component" :space="space" :config="config"></component>
  </div>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';
  import SpaceEditor from 'zero/pages/spaces/views/editor';
  import SpaceList from 'zero/pages/spaces/views/list';

  export default {
    props: [ 'alias' ],

    data: () => ({
      loaded: false,
      component: null,
      space: {},
      config: {}
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
        this.loaded = false;

        SpacesApi.getByAlias(this.alias).then(response =>
        {
          this.space = response;

          if (this.space.view === 'editor')
          {
            this.component = SpaceEditor;
          }
          else if (this.space.view === 'list')
          {
            this.component = SpaceList;
          }
          else
          {
            throw "Not implemented. Custom space view";
          }

          this.loaded = true;
        });
      }

    }
  }
</script>
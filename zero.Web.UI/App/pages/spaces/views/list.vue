<template>
  <div v-if="!loading" class="space-list">
    <ui-header-bar :title="space.name" :count="count" title-empty="List">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" :decision="canCreateShared" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" :config="listRenderer" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';
  import SpacesDefaultList from 'zero/renderers/lists/spaces.default.js';

  export default {
    props: [ 'space', 'config' ],

    data: () => ({
      count: 0,
      loading: true,
      listRenderer: null,
      createRoute: {
        name: 'space-create',
        params: { alias: null }
      },
      canCreateShared: false
    }),

    watch: {
      'space': 'load'
    },

    created()
    {
      this.load();
    },


    methods: {

      load()
      {
        this.loading = true;

        const alias = 'spaces.' + this.space.alias;
        const listRenderer = this.zero.getList(alias) || SpacesDefaultList;

        this.canCreateShared = this.space.allowShared;
        this.createRoute.params.alias = this.space.alias;

        listRenderer.link = item =>
        {
          return {
            name: 'space-item',
            params: { alias: this.space.alias, id: item.id }
          };
        };

        listRenderer.onFetch(q => SpacesApi.getList(this.space.alias, q));

        this.listRenderer = listRenderer;

        this.loading = false;
      },


      add()
      {
        this.$router.push({
          name: 'space-create',
          params: { alias: this.space.alias }
        });
      }
    }
  }
</script>
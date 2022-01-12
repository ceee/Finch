<template>
  <div v-if="!loading" class="space-list" :data-space="space.alias">
    <ui-header-bar :title="space.name" :count="count" title-empty="List">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" :config="listRenderer" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  import api from '../api';
  import { defineComponent } from 'vue';

  export default defineComponent({
    props: [ 'space', 'config' ],

    data: () => ({
      count: 0,
      loading: true,
      listRenderer: null,
      createRoute: {
        name: 'spaces-edit',
        params: { alias: null, id: 'create' }
      }
    }),

    watch: {
      'space': 'load'
    },

    created()
    {
      this.load();
    },


    methods: {

      async load()
      {
        this.loading = true;

        let alias = this.space.alias.indexOf('spaces:') === 0 ? this.space.alias : 'spaces:' + this.space.alias;
        const listRenderer = await this.zero.getSchema(alias) || await this.zero.getSchema('spaces:default');

        this.createRoute.params.alias = this.space.alias;

        listRenderer.link = item =>
        {
          return {
            name: 'spaces-edit',
            params: { alias: this.space.alias, id: item.id }
          };
        };

        listRenderer.onFetch(q => api.getByAlias(this.space.alias, q));

        this.listRenderer = listRenderer;

        this.loading = false;
      },


      add()
      {
        this.$router.push({
          name: 'spaces-edit',
          params: { alias: this.space.alias }
        });
      }
    }
  })
</script>
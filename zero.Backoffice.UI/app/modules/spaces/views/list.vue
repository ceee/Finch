<template>
  <div v-if="!loading" class="space-list" :data-space="alias">
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
    props: [ 'alias' ],

    data: () => ({
      count: 0,
      loading: true,
      listRenderer: null,
      createRoute: {
        name: 'spaces-list-edit',
        params: { alias: null, id: 'create' }
      },
      space: {}
    }),

    created()
    {
      this.load();
    },


    methods: {

      async load()
      {
        this.loading = true;

        api.getTypes().then(async res =>
        {
          this.space = res.data.find(x => x.alias == this.alias);

          let alias = this.alias.indexOf('spaces:') === 0 ? this.alias : 'spaces:' + this.alias;
          const listRenderer = await this.zero.getSchema(alias) || await this.zero.getSchema('spaces:default');

          this.createRoute.params.alias = this.alias;

          listRenderer.link = item =>
          {
            return {
              name: 'spaces-list-edit',
              params: { alias: this.alias, id: item.id }
            };
          };

          listRenderer.onFetch(q => api.getByAlias(this.alias, q));

          this.listRenderer = listRenderer;

          this.loading = false;
        });
      },


      add()
      {
        this.$router.push({
          name: 'spaces-edit',
          params: { alias: this.alias }
        });
      }
    }
  })
</script>
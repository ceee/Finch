<template>
  <div v-if="!loading" class="space-list">
    <ui-header-bar :title="space.name" title-empty="List">
      <ui-table-filter v-model="tableConfig" />
      <ui-add-button :route="createRoute" :decision="canCreateShared" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';

  export default {
    props: [ 'space', 'config' ],

    data: () => ({
      loading: true,
      tableConfig: {},
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

        const alias = 'space.' + this.space.alias;
        let renderer = zero.renderers[alias];

        this.canCreateShared = this.space.allowShared;
        this.createRoute.params.alias = this.space.alias;

        this.tableConfig = renderer && typeof renderer.list === 'object' ? renderer.list : {
          columns: {
            name: {
              as: 'text',
              label: '@ui.name',
              bold: true,
              link: item =>
              {
                return {
                  name: 'space-item',
                  params: { alias: this.space.alias, id: item.id }
                };
              }
            },
            createdDate: {
              as: 'date',
              label: '@ui.createdDate'
            },
            isActive: {
              as: 'bool',
              label: '@ui.active',
              width: 200
            }
          }
        };

        this.tableConfig.items = SpacesApi.getList.bind(this, this.space.alias);

        const firstKey = Object.keys(this.tableConfig.columns)[0];
        this.tableConfig.columns[firstKey].shared = true;

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
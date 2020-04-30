<template>
  <div class="list">
    <ui-header-bar :title="collection.name" title-empty="List">
      <ui-table-filter v-model="tableConfig" />
      <ui-button label="Add item" icon="fth-plus" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import ListsApi from 'zero/resources/spaces.js';

  export default {
    props: [ 'alias' ],

    data: () => ({
      collection: {},
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
        ListsApi.getCollectionByAlias(this.alias).then(response =>
        {
          this.collection = response;
        });

        this.tableConfig = {
          //items: ListsApi.getAll.bind(this, this.alias),
          columns: {
            name: {
              as: 'text',
              label: '@ui.name'
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
      }

    }
  }
</script>
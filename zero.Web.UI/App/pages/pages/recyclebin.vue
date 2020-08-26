<template>
  <div class="pages-recyclebin">
    <ui-header-bar title="@recyclebin.name" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-button type="light" label="@recyclebin.purge" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import RecycleBinApi from 'zero/resources/recycle-bin.js'
  import Overlay from 'zero/services/overlay.js'
  import RecycleBinActionsOverlay from './recyclebin-actions'

  export default {
    data: () => ({
      tableConfig: {}
    }),

    created()
    {
      this.tableConfig = {
        labelPrefix: '@recyclebin.fields.',
        allowOrder: false,
        search: null,
        columns: {
          name: {
            label: '@ui.name',
            as: 'text',
            action: item => this.actions(item)
          },
          originalId: {
            as: 'text',
            width: 300
          },
          createdDate: {
            as: 'datetime',
            width: 200
          },
        },
        items: filter =>
        {
          filter.group = "pages";
          return RecycleBinApi.getByQuery(filter);
        }
      };
    },


    methods: {
      actions(item)
      {
        return Overlay.open({
          component: RecycleBinActionsOverlay,
          display: 'editor',
          model: item
        }).then(value =>
        {
          //EventHub.$emit('page.update');
          //this.$router.push({
          //  name: 'page',
          //  params: { id: value.id }
          //});
        });
      },
    }
  }
</script>
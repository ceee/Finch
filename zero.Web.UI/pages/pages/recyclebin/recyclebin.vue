<template>
  <div class="pages-recyclebin">
    <ui-header-bar title="@recyclebin.name" :count="count" :back-button="true">
      <ui-table-filter :config="tableConfig" />
      <ui-button type="light onbg" label="@recyclebin.purge" @click="purge" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" :config="tableConfig" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  const GROUP = "pages";

  import RecycleBinApi from '@zero/resources/recycle-bin.js'
  import Overlay from '@zero/services/overlay.js'
  import RecycleBinActionsOverlay from './recyclebin-actions.vue'
  import EventHub from '@zero/services/eventhub.js'

  export default {
    data: () => ({
      count: 0,
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
          filter.group = GROUP;
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
          const deleted = !!value.deleted;

          // go to restored page
          if (!deleted)
          {
            EventHub.$emit('page.update');
            this.$router.push({
              name: 'page',
              params: { id: item.originalId }
            });
          }
          // reload recycle bin
          else
          {
            this.$refs.table.update();
          }
        });
      },


      purge()
      {
        Overlay.confirmDelete().then(opts =>
        {
          opts.state('loading');

          RecycleBinApi.deleteByGroup(GROUP).then(res =>
          {
            opts.state('success');
            opts.hide();
            this.$refs.table.update();
          });
        }); 

        
      }
    }
  }
</script>
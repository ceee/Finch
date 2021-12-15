<template>
  <div class="countries">
    <ui-header-bar title="@country.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" alias="countries" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="countries" @count="count = $event" />
    </div>
  </div>
</template>


<script lang="ts">
  export default {
    data: () => ({
      count: 0,
      createRoute: 'countries-edit'
    })
   
    //methods: {
    //  async onAdd(flavor)
    //  {
    //    const model = await api.getEmpty(flavor);

    //    console.info('in');

    //    const result = await overlays.open({
    //      component: () => import('../../editor/editor-overlay.vue'),
    //      display: 'editor',
    //      width: 620,
    //      model: {
    //        title: '@addoverlay.title',
    //        editor: 'countries:add',
    //        entity: model.data,
    //        create: true,
    //        confirmButton: '@ui.create'
    //      },
    //    });

    //    console.info({ model, result });
    //  }
    //}
  }
</script>
<template>
  <div class="countries">
    <ui-header-bar title="@country.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" blueprint-alias="country" />
      <ui-button type="accent" @click="loadData()" label="load" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <!--<ui-table ref="table" config="countries" @count="count = $event" />-->
    </div>
  </div>
</template>


<script lang="ts">
  import { defineComponent } from 'vue';

  export default defineComponent({
    data: () => ({
      count: 0,
      createRoute: 'countries-create'
    }),

    methods: {
      async loadData()
      {
        var schema = await this.zero.getSchema('country');
        console.info(schema);
      }
    }
  });
</script>
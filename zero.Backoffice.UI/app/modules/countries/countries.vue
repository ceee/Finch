<template>
  <div class="countries">
    <ui-header-bar title="@country.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" blueprint-alias="country" />
      <ui-button type="accent" @click="showNotification()" label="load" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <!--<ui-table ref="table" config="countries" @count="count = $event" />-->
    </div>
  </div>
</template>


<script lang="ts">
  import { defineComponent } from 'vue';
  import * as notifications from '../../services/notification';

  export default defineComponent({
    data: () => ({
      count: 0,
      createRoute: 'countries-create'
    }),

    methods: {
      showNotification()
      {
        notifications.error("Erfolgreich", "Super, du hast deine erste Mitteilung veröffentlicht", { duration: 6000 });
      },

      async loadData()
      {
        var schema = await this.zero.getSchema('country');
        console.info(schema);
      }
    }
  });
</script>
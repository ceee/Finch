<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :count="count" :back-button="true" />
    <div class="ui-blank-box">
      <applications-items v-model:items="items" />
    </div>
  </div>
</template> 


<script>
  import ApplicationsItems from '@zero/pages/settings/applications-items.vue';
  import ApplicationsApi from '@zero/resources/applications.js';
  import { ref, onBeforeMount } from 'vue';

  export default {
    components: { ApplicationsItems },

    setup()
    {
      const count = ref(0);
      const items = ref([]);

      onBeforeMount(async () =>
      {
        const res = await ApplicationsApi.getAll();
        count.value = res.totalItems;
        items.value = res.items;
      });

      return {
        count,
        items
      };
    }
  }
</script>


<style lang="scss">
  .apps .apps-items
  {
    margin-top: 0;
  }
</style>
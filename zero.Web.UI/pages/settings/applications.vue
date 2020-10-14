<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :count="count" :back-button="true" />
    <div class="ui-blank-box">
      <applications-items v-model:items="apps" />
    </div>
  </div>
</template> 


<script>
  import ApplicationsItems from '@zero/pages/settings/applications-items.vue'
  import ApplicationsApi from '@zero/resources/applications.js'

  export default {
    data: () => ({
      count: 0,
      apps: []
    }),

    components: { ApplicationsItems },

    mounted()
    {
      ApplicationsApi.getAll().then(response =>
      {
        this.apps = response.items;
        this.count = response.totalItems;
      });
    }
  }
</script>


<style lang="scss">
  .apps .apps-items
  {
    margin-top: 0;
  }
</style>
<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :count="count" :back-button="true" />
    <div class="ui-blank-box">
      <application-items v-model="apps" />
    </div>
  </div>
</template>


<script>
  import ApplicationItems from './partials/application-items.vue';
  import api from './api';

  export default {
    data: () => ({
      count: 0,
      apps: []
    }),

    components: { ApplicationItems },

    mounted()
    {
      api.getByQuery({ pageSize: 1_000 }).then(response =>
      {
        this.apps = response.data;
        this.count = response.data.length;
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
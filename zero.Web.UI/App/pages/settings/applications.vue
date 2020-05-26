<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :back-button="true" />
    <div class="ui-blank-box">
      <applications-items v-model="apps" />
    </div>
  </div>
</template>


<script>
  import ApplicationsItems from 'zero/pages/settings/applications-items'
  import ApplicationsApi from 'zero/resources/applications.js'

  export default {
    data: () => ({
      apps: []
    }),

    components: { ApplicationsItems },

    mounted()
    {
      ApplicationsApi.getAll().then(response =>
      {
        this.apps = response.items;
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
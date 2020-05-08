<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :back-button="true" />
    <div class="ui-blank-box">
      <applications-items />
    </div>
  </div>
</template>


<script>
  import ApplicationsItems from 'zero/pages/settings/applications-items'

  export default {
    data: () => ({
      apps: []
    }),

    components: { ApplicationsItems }
  }
</script>


<style lang="scss">
  .apps .apps-items
  {
    margin-top: 0;
  }
</style>
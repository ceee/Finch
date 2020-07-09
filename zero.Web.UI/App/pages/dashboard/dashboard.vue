<template>
  <div class="dashboard">
    <div class="dashboard-elements">
      <dashboard-element :high="true" />
      <dashboard-element :wide="true" />
      <dashboard-element />
      <dashboard-element />
      <dashboard-element />
    </div>
  </div>
</template>

<script>
  import DashboardElement from 'zero/dashboard/element';

  export default {
    components: { DashboardElement },
  }
</script>

<style lang="scss">
  .dashboard
  {
    position: relative;
    padding-top: 68px;
  }

  .dashboard-elements
  {
    display: grid;
    padding: var(--padding);
    grid-gap: var(--padding);
    grid-template-columns: repeat(auto-fill, minmax(360px, 1fr));
    grid-auto-rows: minmax(240px, auto);
    grid-auto-flow: dense;
  }
</style>
<template>
  <div class="media-overview-drop">
    <ui-icon symbol="fth-upload" :size="26" :stroke-width="2" />
  </div>
</template>

<script>
  export default {
    name: 'mediaOverviewDrop',

    props: {
      
    }
  };
</script>

<style lang="scss">
  .media-overview-drop
  {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    min-height: 130px;
    border-radius: var(--radius);
    align-self: flex-start;
    box-shadow: var(--shadow-short);
    border: 1px dashed var(--color-line-dashed-onbg);
    color: var(--color-text);
  }

  .ui-datagrid-outer.is-selecting .media-overview-drop
  {
    pointer-events: none;
    opacity: .55;
  }
</style>

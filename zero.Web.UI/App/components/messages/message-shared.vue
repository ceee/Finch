<template>
  <ui-property class="ui-isshared" label="Shared" :vertical="true" :is-text="true">
    <div class="ui-isshared-inner" title="This entity is shared and can be used be all applications">
      <i class="fth-check ui-isshared-icon"></i> <span class="ui-isshared-text">True</span>
    </div>
  </ui-property>
</template>


<script>
  export default {
    name: 'uiShared'
  }
</script>

<style lang="scss">
  .ui-isshared-inner
  {
    cursor: help;
  }


  .ui-isshared-icon
  {
    display: inline-block;
    background: var(--color-bg-light);
    width: 24px;
    height: 24px;
    line-height: 24px;
    border-radius: 12px;
    box-shadow: var(--color-shadow-short);
    text-align: center;
    margin-right: 6px;
    font-size: var(--font-size-s);
    color: var(--color-fg);
  }

  .ui-isshared-text
  {

  }
</style>
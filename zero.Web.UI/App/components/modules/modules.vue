<template>
  <div class="ui-modules-inner">
    <ui-modules-select v-if="canAdd" />
  </div>
</template>


<script>
  import ModulesApi from 'zero/resources/modules.js';

  export default {
    name: 'uiModules',

    props: {
      value: [String, Object, Array],
      config: Object
    },


    data: () => ({
      canAdd: true,
      isSelecting: false,
      moduleTypes: []
    }),


    methods: {

      start()
      {
        this.isSelecting = true;
      }

    }
  }
</script>

<style lang="scss">
  @import './Sass/Modules/box';

  .ui-property.ui-modules
  {
    width: 100%;
    margin: 0;
    margin-top: var(--padding);
  }

  .ui-modules-start
  {
    margin: 0;
    padding: var(--padding);
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    box-shadow: var(--color-shadow-short);
    display: flex;
    justify-content: center;
  }

  .ui-modules-start-button
  {
    color: var(--color-fg);
    font-size: var(--font-size);
    display: inline-grid;
    grid-template-columns: auto 1fr;
    grid-gap: 25px;
    align-items: center;
  }

  .ui-modules-start-button-icon
  {
    width: 70px;
    height: 70px;
    line-height: 68px !important;
    font-size: 20px;
    text-align: center;
    background: var(--color-bg-bright-two);
    border-radius: var(--radius);
  }

  .ui-modules-start-button-text
  {
    line-height: 1.3;
    color: var(--color-fg-dim);

    strong
    {
      display: inline-block;
      margin-bottom: 5px;
      color: var(--color-fg);
    }

  }
</style>
<template>
  <div class="ui-modules-inner">
     <div class="ui-modules-start">
       <button type="button" class="ui-modules-start-button">
         <i class="ui-modules-start-button-icon fth-plus"></i>
         <p class="ui-modules-start-button-text"><strong>Add content</strong> <br>Compose the page by adding modules</p>
       </button>
     </div>
  </div>
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object
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
<template>
  <div class="ui-header-bar">
    <div class="ui-header-bar-main">
      <ui-icon-button v-if="onBack" :click="onBack" />
      <h2 v-if="title" class="ui-header-bar-title" v-localize="title"></h2>
    </div>
    <div class="ui-header-bar-aside">
      <slot></slot>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiHeaderBar',

    props: {
      title: {
        type: String
      },
      onBack: {
        type: Function
      }
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>

<style lang="scss">
  @import 'Sass/Core/all';

  .ui-header-bar
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    height: 80px;
    padding: 0 var(--padding);
    background: var(--color-bg-light);
  }

  .ui-header-bar-main
  {
    display: flex;
    align-items: center;
    height: 100%;

    .ui-icon-button
    {
      margin-right: var(--padding);
    }
  }

  .ui-header-bar-aside
  {
    display: flex;
    align-items: center;
    height: 100%;
  }

  .ui-header-bar-title
  {
    @extend %font;
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 700;
  }
</style>
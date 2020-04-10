<template>
  <div class="ui-header-bar">
    <div class="ui-header-bar-main">
      <ui-icon-button v-if="onBack" :click="onBack" />
      <h2 v-if="title" class="ui-header-bar-title" v-localize="title"></h2>
      <h2 v-if="!title && titleEmpty" class="ui-header-bar-title is-empty" v-localize="titleEmpty"></h2>
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
      titleEmpty: {
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
  .ui-header-bar
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    height: 80px;
    padding: 0 var(--padding);
    background: var(--color-bg-light);
    //border-bottom: 1px solid var(--color-line);
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

    > * + *
    {
      margin-left: 15px;
    }
  }

  .ui-header-bar-title
  {
    font-family: var(--font);
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 700;

    &.is-empty
    {
      color: var(--color-fg-light);
      font-weight: 400;
    }
  }
</style>
<template>
  <div class="ui-header-bar">
    <div class="ui-header-bar-main">
      <ui-icon-button v-if="backButton" @click="onBack" />
      <div>
        <h2 v-if="title" class="ui-header-bar-title" v-localize="title"></h2>
        <h2 v-if="!title && titleEmpty" class="ui-header-bar-title is-empty" v-localize="titleEmpty"></h2>
        <p v-if="description" class="ui-header-bar-description" v-localize="description"></p>
      </div>
    </div>
    <div class="ui-header-bar-aside">
      <slot></slot>
      <ui-icon-button v-if="closeButton" @click="onClose" icon="fth-x" title="@ui.close" />
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay';

  export default {
    name: 'uiHeaderBar',

    props: {
      title: {
        type: String
      },
      titleEmpty: {
        type: String
      },
      description: {
        type: String
      },
      backButton: {
        type: Boolean,
        default: false
      },
      closeButton: {
        type: Boolean,
        default: false
      }
    },

    mounted ()
    {
      
    },

    methods: {
      onBack()
      {
        this.$router.go(-1);
      },
      onClose()
      {
        Overlay.close();
      }
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
    color: var(--color-fg);
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 700;

    &.is-empty
    {
      color: var(--color-fg-light);
      font-weight: 400;
    }
  }

  .ui-header-bar-description
  {
    font-size: var(--font-size-s);
    color: var(--color-fg-light);
    margin: 2px 0 0;
  }
</style>
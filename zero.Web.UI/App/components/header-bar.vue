<template>
  <div class="ui-header-bar">
    <div class="ui-header-bar-inner">
      <div class="ui-header-bar-main">
        <ui-icon-button v-if="backButton" type="light onbg" @click="onBack" />
        <div class="ui-header-bar-main-title">
          <slot name="title">
            <h2 class="ui-header-bar-title" :class="{'is-empty': !title && titleEmpty}">
              <span v-if="prefix" class="-minor" v-localize:html="prefix"></span>
              <span v-localize="title || titleEmpty"></span>
              <span v-if="suffix" class="-minor" v-localize:html="suffix"></span>
            </h2>
          </slot>
          <p v-if="description" class="ui-header-bar-description" v-localize="description"></p>
        </div>
      </div>
      <div class="ui-header-bar-aside">
        <slot></slot>
        <ui-icon-button v-if="closeButton" @click="onClose" icon="fth-x" title="@ui.close" />
      </div>
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
      prefix: {
        type: String
      },
      suffix: {
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
    width: 100%;
    height: 100px;
    padding: 0 var(--padding) 0; //10px;

    & + .ui-blank-box, & + .ui-box, & + .ui-view-box
    {
      margin-top: 0;
    }

    & + .ui-view-box
    {
      padding-top: 0;
    }

    .app-tree &
    {
      height: 90px;
      margin-bottom: 10px;
    }
  }

  .ui-header-bar-inner
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    /*max-width: 1104px;*/
  }

  .ui-header-bar-main
  {
    display: flex;
    align-items: center;
    height: 100%;
    flex: 1 0 auto;

    .ui-icon-button
    {
      margin-right: 16px;
    }
  }

  .ui-header-bar-main-title
  {
    flex: 1 0 auto;
  }

  .ui-header-bar-aside
  {
    display: flex;
    align-items: center;
    height: 100%;
    flex-shrink: 0;
    padding-left: 40px;

    > * + *
    {
      margin-left: 15px;
    }
  }

  .ui-header-bar-title
  {
    font-family: var(--font);
    color: var(--color-text);
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 700;

    &.is-empty, .-minor
    {
      color: var(--color-text-dim);
      font-weight: 400;
    }
  }

  .ui-header-bar-description
  {
    font-size: var(--font-size-s);
    color: var(--color-text-dim);
    margin: 2px 0 0;
  }
</style>
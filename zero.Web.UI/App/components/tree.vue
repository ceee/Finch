<template>
  <div class="ui-tree" :style="{ 'padding-left': depth > 0 ? ((depth * 18) + 'px') : null }">
    <span v-if="status === 'loading'" class="ui-tree-item-loading"><i></i></span>
    <template v-for="item in items">
      <div class="ui-tree-item" :class="getClasses(item)">
        <button v-if="item.hasChildren" @click="toggle(item)" type="button" class="ui-tree-item-toggle">
          <i class="ui-tree-item-arrow" :class="['fth-chevron-' + (item.isOpen ? 'up' : 'down')]"></i>
        </button>
        <router-link :to="item.url" class="ui-tree-item-link">
          <i class="ui-tree-item-icon" :class="item.icon"></i>
          <i v-if="item.modifier" :title="item.modifier.name" class="ui-tree-item-modifier" :class="item.modifier.icon"></i>
          <span class="ui-tree-item-text">{{item.name | localize}}</span>
        </router-link>
        <ui-dot-button class="ui-tree-item-actions" v-if="configuration.onActionsRequested" />
      </div>
      <ui-tree v-if="item.hasChildren && item.isOpen" :get="get" :parent="item.id" :depth="depth + 1" :active="active" :config="config" />
    </template>
  </div>
</template>


<script>
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  const defaultConfig = {
    // return actions for an item
    onActionsRequested: null
  };

  export default {
    name: 'uiTree',

    props: {
      depth: {
        type: Number,
        default: 0
      },
      active: {
        type: String,
        default: null
      },
      parent: {
        type: String,
        default: null
      },
      get: {
        type: Function,
        required: true
      },
      config: {
        type: Object,
        default: () =>
        {
          return defaultConfig;
        }
      }
    },

    data: () => ({
      items: [],
      status: 'none',
      actions: [],
      configuration: {}
    }),

    mounted()
    {
      this.initialize();
      this.load(this.parent);
    },

    methods: {

      initialize()
      {
        this.configuration = _extend(defaultConfig, this.config);
      },

      // loads children of the given parent id or on root if empty
      load(parent)
      {
        this.setStatus('loading', this.items);

        let promise = this.get(parent, this.active);

        promise.then(response =>
        {
          this.items = response;
          this.setStatus('loaded', this.items);
        })
        .catch(error =>
        {
          this.setStatus('error', this.items, error);
          // TODO handle errors
        });
      },


      // updates the status for the current tree
      setStatus(status)
      {
        this.status = status;
        this.$emit('onStatusChange', status);
      },


      // toggles children of an item
      toggle(item)
      {
        item.isOpen = !item.isOpen;
      },


      // get all classes for a tree item
      getClasses(item)
      {
        return {
          'has-children': item.hasChildren,
          'is-inactive': item.isInactive,
          'is-open': item.isOpen
        };
      }

    }
  }
</script>


<style lang="scss">
  .ui-tree
  {
    position: relative;
  }

  .ui-tree-item
  {
    display: grid;
    grid-template-columns: 1fr auto;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 var(--padding);
    height: 50px;
    color: var(--color-fg);
    position: relative;
    transition: color 0.2s ease;

    &.is-inactive .ui-tree-item-text
    //&.is-inactive .ui-tree-item-icon
    {
      opacity: .5;
    }

    .ui-tree-item-arrow
    {
      transition: transform 0.2s ease;
    }

    &:hover > .ui-tree-item-actions
    {
      transition-delay: 0.2s;
      opacity: 1;
    }
    

    &.is-open > .ui-tree-item-toggle .ui-tree-item-arrow
    {
      transform: rotate(180deg);
    }
  }

  .ui-tree-item-link
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    grid-gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;
    color: var(--color-fg);

    &.is-active
    {
      font-weight: bold;
    }
  }

  .ui-tree-item-toggle
  {
    position: absolute;
    color: var(--color-fg-mid);
    height: 100%;
    top: 0;
    left: 0;
    width: 30px;
    text-align: right;
    padding-right: 5px;
    outline: none !important;
    transition: color 0.2s ease;

    &:hover
    {
      color: var(--color-fg);
    }
  }

  .ui-tree-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-fg-reverse-mid);
    transition: color 0.2s ease;
  }

  .ui-tree-item-loading
  {
    display: block;
    overflow: hidden;
    position: absolute;
    left: 0;
    right: 0;
    height: 2px;

    i
    {
      background-color: var(--color-line);
      transform: translateX(-100%) scaleX(1);
      animation: treeitemloading 1s linear infinite;
      width: 100%;
      height: 100%;
      position: absolute;
      left: 0;
      top: 0;
    }
  }

  @keyframes treeitemloading
  {
    0%  { transform: translateX(-100%); }
    100% { transform: translateX(100%); }
  }

  .ui-tree-item-modifier
  {
    position: absolute;
    left: 10px;
    bottom: 12px;
    background: var(--color-bg-light);
    border-radius: 50%;
    width: 14px;
    height: 14px;
    /*color: var(--color-fg-mid);*/
    font-size: 11px;
    font-style: normal;
    text-align: center;
    line-height: 14px;
  }

  .ui-tree-item-actions
  {
    transition: opacity 0.2s ease 0;
    opacity: 0;
    color: var(--color-fg-mid);
  }
</style>
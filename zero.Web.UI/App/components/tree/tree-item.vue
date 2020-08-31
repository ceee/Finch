<template>
  <div class="ui-tree-item" :class="getClasses(value)" v-on:contextmenu="onRightClicked(value, $event)">
    <button :disabled="value.disabled" v-if="value.hasChildren" @click="toggle(value)" type="button" class="ui-tree-item-toggle">
      <i class="ui-tree-item-arrow" :class="['fth-chevron-' + (value.isOpen ? 'up' : 'down')]"></i>
    </button>
    <component :disabled="value.disabled" :is="tag" type="button" :to="value.url" class="ui-tree-item-link" @click="onClick(value, $event)">
      <i class="ui-tree-item-icon" :class="value.icon"></i>
      <i v-if="value.modifier" :title="value.modifier.name" class="ui-tree-item-modifier" :class="value.modifier.icon"></i>
      <span class="ui-tree-item-text">{{value.name | localize}}</span>
    </component>
    <ui-dot-button :disabled="value.disabled" class="ui-tree-item-actions" v-if="value.hasActions" @click="onActionsClicked(value, $event)" />
  </div>
</template>


<script>
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {
    name: 'uiTreeItem',

    props: {
      value: {
        type: Object,
        required: true
      }
    },

    computed: {
      isLink()
      {
        return this.value && this.value.url;
      },
      tag()
      {
        return this.isLink ? 'router-link' : 'button';
      }
    },

    methods: {
      onClick(item, ev)
      {
        if (this.isLink)
        {
          return;
        }
        else
        {
          this.$emit('click', item, ev);
        }
      },

      // get all classes for a tree item
      getClasses(item)
      {
        return {
          'has-children': item.hasChildren,
          'is-inactive': item.isInactive,
          'is-open': item.isOpen,
          'is-selected': item.isSelected,
          'is-disabled': item.disabled,
          'is-active': this.isLink && (item.id == this.$route.params.id || (item.url && !item.url.params && item.url.name === this.$route.name))
        };
      },

      // toggles children of an item
      toggle(item)
      {
        this.$emit('open', item);
      },

      // right clicked on an item
      onRightClicked(item, ev)
      {
        if (!item.disabled)
        {
          this.$emit('rightclick', item, ev);
        }
      },

      // actions button clicked on item
      onActionsClicked(item, ev)
      {
        if (!item.disabled)
        {
          this.$emit('actions', item, ev);
        }
      }
    }
  }
</script>


<style lang="scss">

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
    position: relative;

    &.is-inactive .ui-tree-item-text
    //&.is-inactive .ui-tree-item-icon
    {
      color: var(--color-fg-dim);
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

    &.is-disabled
    {
      cursor: not-allowed;
    }

    &.is-active:before
    {
      content: ' ';
      position: absolute;
      top: 0;
      bottom: 0;
      left: -300px;
      right: 0;
      background: var(--color-bg-bright-two);
      border-right: 3px solid var(--color-primary);
    }
  }

  .ui-tree-item-link
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;    
    color: var(--color-fg);

    &:hover
    {
      color: var(--color-fg);
    }

    &.is-active
    {
      color: var(--color-fg);
      font-weight: bold;
      //color: var(--color-primary);
    }
  }

  .ui-tree-item-toggle
  {
    position: absolute;
    color: var(--color-fg-dim);
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
      background-color: var(--color-bg-bright-three);
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
    background: var(--color-bg-bright);
    border-radius: 50%;
    width: 14px;
    height: 14px;
    /*color: var(--color-fg-dim);*/
    font-size: 11px;
    font-style: normal;
    text-align: center;
    line-height: 14px;
  }

  .ui-tree-item-actions
  {
    transition: opacity 0.2s ease 0;
    opacity: 0;
    color: var(--color-fg-dim);
  }
</style>
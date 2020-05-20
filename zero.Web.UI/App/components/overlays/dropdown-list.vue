<template>
  <div class="ui-dropdown-list" :class="{ 'is-multiline': multiline }">
    <template v-for="item in value">
      <button v-if="!item.type || item.type === 'button'" :disabled="item.disabled" type="button" @click="clicked(item)" 
              class="ui-dropdown-list-item" :class="{ 'has-icon': item.icon, 'is-active': item.active || item.isActive }">
        <i v-if="item.icon" class="ui-dropdown-list-item-icon" :class="item.icon" :style="{ color: item.color ? item.color : null }"></i>
        {{item.name | localize}}
        <i v-if="item.active || item.isActive" class="ui-dropdown-list-item-selected fth-check"></i>
        <i v-if="item.loading" class="ui-dropdown-list-item-progress"></i>
      </button>
      <hr class="ui-dropdown-list-separator" v-if="item.type === 'separator'">
    </template>
  </div>
</template>


<script>
  import { warn } from 'zero/services/debug';

  export default {
    name: 'uiDropdownList',

    props: {
      multiline: {
        type: Boolean,
        default: false
      },
      value: {
        type: Array,
        required: true,
        default: []
      },
      action: {
        type: Function,
        default: () => { }
      }
    },

    data: () => ({
      dropdown: null
    }),

    mounted ()
    {
      // find parent dropdown so we can pass it on item click
      let current = this;
      do
      {
        if (current.$options.name === 'uiDropdown')
        {
          this.dropdown = current;
          break;
        }
      }
      while (current = current.$parent);

      if (false && !this.dropdown)
      {
        warn('ui-dropdown-list: Could not find parent <ui-dropdown />');
      }
    },

    methods: {

      clicked(item)
      {
        var instance = this;

        if (!item.loading && !item.disabled)
        {
          let baseObj = typeof item.action === 'function' ? item : this;
          baseObj.action(item, {
            dropdown: this.dropdown,
            hide()
            {
              if (this.dropdown)
              {
                this.dropdown.hide();
              }
              instance.$emit('hide');
            },
            loading(isLoading)
            {
              instance.$set(item, 'loading', isLoading);
            }
          });
        }
      }

    }
  }
</script>


<style lang="scss">
  .ui-dropdown-list
  {
    padding: 5px;
  }

  button.ui-dropdown-list-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto;
    grid-gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 16px;
    height: 46px;
    color: var(--color-fg-mid);
    border-radius: var(--radius);
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
    max-width: 300px;

    &.has-icon
    {
      grid-template-columns: 30px 1fr auto;
    }

    .is-multiline &
    {
      white-space: normal;
      overflow: visible;
    }

    &:not([disabled]):hover, &.is-active, &:focus
    {
      background: var(--color-highlight);
      color: var(--color-fg);
      //font-weight: 700;

      .ui-dropdown-list-item-icon
      {
        color: var(--color-fg);
      }
    }

    &.is-active
    {
      font-weight: 700;
    }

    &[disabled]
    {
      color: var(--color-fg-light);
      cursor: default;
      pointer-events: none;

      .ui-dropdown-list-item-icon
      {
        color: var(--color-fg-light);
      }
    }
  }

  .ui-dropdown-list-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -1px;
    color: var(--color-fg-mid);
    transition: color 0.2s ease;
  }

  .ui-dropdown-list-separator
  {
    border-bottom-color: var(--color-highlight);
    margin: 5px 0;
  }

  .ui-dropdown-list-item-progress
  {   
    width: 16px;
    height: 16px;
    z-index: 2;
    border-radius: 40px;
    border: 2px solid transparent;
    border-left-color: var(--color-fg);
    opacity: 1;
    will-change: transform;
    animation: rotating .5s linear infinite;
    transition: opacity .25s ease;
  }

  @keyframes rotating
  {
    from
    {
      -webkit-transform: rotate(0);
      transform: rotate(0)
    }
    to
    {
      -webkit-transform: rotate(1turn);
      transform: rotate(1turn)
    }
  }
</style>
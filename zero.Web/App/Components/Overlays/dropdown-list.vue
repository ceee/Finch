<template>
  <div class="ui-dropdown-list">
    <template v-for="item in items">
      <button v-if="!item.type || item.type === 'button'" :disabled="item.disabled" type="button" @click="clicked(item)" class="ui-dropdown-list-item">
        <i v-if="item.icon" class="ui-dropdown-list-item-icon" :class="item.icon" :style="{ color: item.color ? item.color : null }"></i>
        {{item.name | localize}}
      </button>
      <hr class="ui-dropdown-list-separator" v-if="item.type === 'separator'">
    </template>
  </div>
</template>


<script>
  export default {
    name: 'uiDropdownList',

    props: {
      items: {
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

    watch: {
      
    },

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

      if (!this.dropdown)
      {
        console.warn('ui-dropdown-list: Could not find parent <ui-dropdown />');
      }
    },

    methods: {

      clicked(item)
      {
        let baseObj = typeof item.action === 'function' ? item : this;
        baseObj.action(item, this.dropdown);
      }

    }
  }
</script>


<style lang="scss">
  .ui-dropdown-list
  {
    padding: 5px;
  }

  .ui-dropdown-list-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 30px 1fr auto;
    grid-gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 16px;
    height: 46px;
    color: var(--color-fg-mid);
    border-radius: var(--radius);

    &:not([disabled]):hover, &.is-active
    {
      background: var(--color-highlight);
      color: var(--color-fg);
      font-weight: 700;

      .ui-dropdown-list-item-icon
      {
        color: var(--color-fg);
      }
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
    margin: 10px 0;
  }
</style>
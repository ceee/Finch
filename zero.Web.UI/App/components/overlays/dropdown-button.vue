<template>
  <button :disabled="disabled" type="button" @click="onClick" 
          class="ui-dropdown-button" :class="{ 'has-icon': icon, 'is-active': selected, 'is-multiline': multiline }">
    <i v-if="icon" class="ui-dropdown-button-icon" :class="icon"></i>
    {{label | localize}}
    <i v-if="selected" class="ui-dropdown-button-selected fth-check"></i>
    <i v-if="loading" class="ui-dropdown-button-progress"></i>
  </button>
</template>


<script>
  import { warn } from 'zero/services/debug';

  export default {
    name: 'uiDropdownButton',

    props: {
      value: {
        default: null
      },
      multiline: {
        type: Boolean,
        default: false
      },
      label: {
        type: String,
        required: true
      },
      icon: {
        type: String
      },
      selected: {
        type: Boolean,
        default: false
      },
      action: {
        type: Function,
        default: () => { }
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      dropdown: null,
      loading: false
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
        warn('ui-dropdown-button: Could not find parent <ui-dropdown />');
      }
    },

    methods: {

      onClick()
      {
        var instance = this;

        if (!this.loading && !this.disabled)
        {
          this.$emit('click', this.value, {
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
              instance.loading = isLoading;
            }
          });
        }
      }

    }
  }
</script>


<style lang="scss">
  button.ui-dropdown-button
  {
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto;
    grid-gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 16px;
    height: 46px;
    color: var(--color-fg);
    border-radius: var(--radius);
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
    max-width: 300px;

    &.has-icon
    {
      grid-template-columns: 30px 1fr auto;
    }

    &.is-multiline
    {
      white-space: normal;
      overflow: visible;
    }

    &:not([disabled]):hover, &.is-active, &:focus
    {
      background: var(--color-bg-bright-three);
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
      color: var(--color-fg-dim);
      cursor: default;
      pointer-events: none;

      .ui-dropdown-list-item-icon
      {
        color: var(--color-fg-dim);
      }
    }

    & + .ui-dropdown-button
    {
      margin-top: 4px;
    }
  }

  .ui-dropdown-button-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -1px;
    color: var(--color-fg);
    transition: color 0.2s ease;
  }

  .ui-dropdown-button-progress
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
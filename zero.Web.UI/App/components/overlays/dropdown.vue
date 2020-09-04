<template>
  <div class="ui-dropdown-container">
    <div v-if="hasButton" class="ui-dropdown-toggle" @click.stop="toggle">
      <slot name="button"></slot>
    </div>
    <div class="ui-dropdown" role="dialog" v-if="open" v-click-outside="hide" :class="dropdownClasses">
      <slot></slot>
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay';

  export default {
    name: 'uiDropdown',

    props: {
      align: {
        type: String,
        default: 'left'
      },
      theme: {
        type: String,
        default: null
      },
      locked: {
        type: Boolean,
        default: false
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    computed: {

      hasButton()
      {
        return this.$scopedSlots.hasOwnProperty('button');
      },

      dropdownClasses()
      {
        let classes = 'align-' + this.align.split(' ').join(' align-');

        if (!!this.theme)
        {
          classes += ' theme-' + this.theme;
        }

        return classes;
      }
    },

    data: () => ({
      open: false
    }),


    methods: {

      toggle()
      {
        if (this.open)
        {
          this.hide();
        }
        else if (!this.disabled)
        {
          this.show();
        }
      },

      show()
      {
        if (this.disabled)
        {
          return;
        }
        Overlay.setDropdown(this);
        this.open = true;
        this.$emit('opened');
      },

      hide()
      {
        if (this.locked)
        {
          return;
        }
        this.open = false;
        this.$emit('closed');
      }

    }
  }
</script>


<style lang="scss">
  .ui-dropdown-container
  {
    position: relative;
  }

  .ui-dropdown
  {
    position: absolute;
    min-width: 300px;
    min-height: 20px;
    background: var(--color-bg-bright-two);
    border-radius: var(--radius);
    border: 1px solid var(--color-line-light);
    box-shadow: 0 0 32px var(--color-shadow);
    z-index: 8;
    top: calc(100% + 5px);
    padding: 5px;
    color: var(--color-fg);

    &.align-right
    {
      right: 0;
    }

    &.align-top
    {
      top: calc(100% + 5px);
      bottom: auto;
    }

    &.align-bottom
    {
      bottom: calc(100% + 5px);
      top: auto;
    }
  }
</style>
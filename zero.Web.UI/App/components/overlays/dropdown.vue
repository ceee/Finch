<template>
  <div class="ui-dropdown-container">
    <div class="ui-dropdown-toggle" @click.stop="toggle">
      <slot name="button"></slot>
    </div>
    <div class="ui-dropdown theme-dark" role="dialog" v-if="open" v-click-outside="hide" :class="alignClasses">
      <slot></slot>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiDropdown',

    props: {
      align: {
        type: String,
        default: 'left'
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    computed: {

      alignClasses()
      {
        return 'align-' + this.align.split(' ').join(' align-');
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
        else
        {
          this.show();
        }
      },

      show()
      {
        this.open = true;
      },

      hide()
      {
        this.open = false;
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
    background: var(--color-bg-mid);
    border-radius: var(--radius);
    border: 1px solid var(--color-line);
    /*box-shadow: 0 0 20px var(--color-shadow);*/
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
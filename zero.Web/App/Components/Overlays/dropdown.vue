<template>
  <div class="ui-dropdown-container">
    <div class="ui-dropdown-toggle" @click.stop.prevent="toggle">
      <slot name="button"></slot>
    </div>
    <div class="ui-dropdown" role="dialog" v-if="open" v-click-outside="hide">
      <slot></slot>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiDropdown',

    props: {
      
    },

    data: () => ({
      open: false
    }),

    watch: {
      
    },

    mounted ()
    {
      
    },

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
    min-height: 200px;
    background: var(--color-bg-mid);
    border-radius: var(--radius);
    border: 1px solid var(--color-line);
    box-shadow: 0 0 20px var(--color-shadow);
    z-index: 8;
    top: calc(100% + 5px);
  }
</style>
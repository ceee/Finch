<template>
  <div class="ui-context-button">
    <ui-button :submit="true" label="@ui.save" :state="state" />
    <ui-dropdown ref="dropdown" align="right">
      <template v-slot:button>
        <ui-button icon="fth-chevron-down" />
      </template>
      <slot></slot>
    </ui-dropdown>
  </div>
</template>


<script>
  import { find as _find } from 'underscore';

  export default {
    props: {
      label: {
        type: String,
        default: '@ui.save'
      },
      state: {
        type: String,
        default: 'default'
      },
      type: {
        type: String,
        default: 'action'
      },
      route: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    methods: {

      onClick(isShared)
      {
        
      }

    }

  }
</script>

<style lang="scss">
  .ui-context-button
  {
    display: flex;

    > .ui-button
    {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
    }

    .ui-dropdown-toggle .ui-button
    {
      padding: 0 14px;
      border-top-left-radius: 0;
      border-bottom-left-radius: 0;
      border-left: 1px solid var(--color-primary-line);
    }
  }

  .ui-context-button .ui-button + .ui-dropdown-container
  {
    margin-left: 0;
  }
</style>
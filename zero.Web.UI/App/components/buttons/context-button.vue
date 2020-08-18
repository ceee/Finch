<template>
  <div class="ui-context-button">
    <ui-dropdown ref="dropdown" align="right">
      <template v-slot:button>
        <ui-button type="light" icon="fth-chevron-down" />
      </template>
      <slot></slot>
    </ui-dropdown>
    <ui-button :submit="true" label="@ui.save" :state="state" />
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
    background: var(--color-bg-bright-two);
    border-radius: 10px;
    padding: 5px;

    .ui-dropdown-toggle .ui-button
    {
      padding: 0 26px 0 12px;
      margin-right: -6px;
    }
  }

  .ui-context-button .ui-button + .ui-dropdown-container
  {
    margin-left: 0;
  }
</style>
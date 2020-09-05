<template>
  <ui-header-bar class="ui-form-header" :title="value.name" :title-empty="title" :back-button="true">
    <div class="ui-form-header-aside">
      <slot></slot>
      <div v-if="activeToggle" class="ui-form-header-toggle">
        <ui-toggle v-model="value.isActive" class="is-primary" off-content="@ui.inactive" :off-warning="true" on-content="@ui.active" :content-left="true" />
      </div>
      <ui-dropdown v-if="!isCreate" align="right">
        <template v-slot:button>
          <ui-button type="white" label="@ui.actions" caret="down" />
        </template>
        <slot name="actions"></slot>
        <ui-dropdown-button v-if="canDelete" label="@ui.delete" icon="fth-trash" @click="onDelete" :disabled="disabled" />
      </ui-dropdown>
      <ui-button :submit="true" label="@ui.save" :state="state" v-if="!disabled" />
    </div>
  </ui-header-bar>
</template>


<script>
  export default {
    name: 'uiFormHeader',

    props: {
      title: {
        type: String,
        default: null
      },
      value: {
        type: Object
      },
      canDelete: {
        type: Boolean,
        default: true
      },
      disabled: {
        type: Boolean,
        default: false
      },
      state: {
        type: String
      },
      isCreate: {
        type: Boolean,
        default: false
      },
      activeToggle: {
        type: Boolean,
        default: false
      }
    },

    methods: {
      onDelete(item, opts)
      {
        this.$emit('delete', item, opts);
      }
    }
  }
</script>


<style lang="scss">
  .ui-form-header-aside
  {
    //background: var(--color-bg-bright);
    //border-radius: var(--radius);
    display: flex;
    align-items: center;
    justify-content: flex-end;
    //padding: 5px 5px 5px 24px;

    > * + *
    {
      margin-left: 16px;
    }
    
    /*.ui-dropdown-toggle > button
    {
      background: none;

      > .ui-button-caret
      {
        margin-left: 10px;
      }
    }*/
  }

  .ui-dropdown-separator
  {
    border-bottom-color: var(--color-highlight);
    margin: 5px 0;
  }

  .ui-form-header-toggle
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    position: relative;
    top: -1px;
    /*background: var(--color-bg-bright);
    color: var(--color-fg);
    border: none;
    box-shadow: var(--color-shadow-short);
    border-radius: var(--radius);

    .ui-toggle-text
    {
      margin-right: 20px !important;
    }*/
    .ui-toggle-switch
    {
      background: var(--color-bg-bright);
      box-shadow: var(--color-shadow-short);
    }

    .ui-toggle-off-warning
    {
      color: var(--color-accent-red);
    }
  }
</style>
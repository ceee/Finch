<template>
  <ui-header-bar :title="value.name" :title-empty="title" :back-button="true">
    <slot></slot>
    <div v-if="activeToggle" class="ui-header-bar-toggle">
      <ui-toggle v-model="value.isActive" class="is-primary" off-content="@ui.inactive" on-content="@ui.active" :content-left="true" />
    </div>
    <ui-context-button :state="state">
      <div v-if="!isCreate">
        <slot name="actions"></slot>
        <ui-dropdown-button v-if="canDelete" label="@ui.delete" icon="fth-trash" @click="onDelete" :disabled="disabled" />
      </div>
    </ui-context-button>
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
  .ui-dropdown-separator
  {
    border-bottom-color: var(--color-highlight);
    margin: 5px 0;
  }

  .ui-header-bar-toggle
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    padding: 0 14px 0 20px;
    height: 42px;
    /*background: var(--color-bg-bright);
    color: var(--color-fg);
    border: none;
    box-shadow: var(--color-shadow-short);
    border-radius: var(--radius);

    .ui-toggle-text
    {
      margin-right: 20px !important;
    }*/
  }
</style>
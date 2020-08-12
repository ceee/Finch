<template>
  <ui-header-bar :title="value.name" :title-empty="title" :back-button="true">
    <!--<ui-dropdown align="right" :disabled="isCreate">
      <template v-slot:button>
        <ui-button type="white" label="<span>Language:</span> English" caret="down" />
      </template>
    </ui-dropdown>-->
    <slot></slot>
    <ui-dropdown v-if="!isCreate" align="right">
      <template v-slot:button>
        <ui-button type="white" label="@ui.actions" caret="down" />
      </template>
      <slot name="actions"></slot>
      <ui-dropdown-button v-if="canDelete" label="@ui.delete" icon="fth-trash" @click="onDelete" :disabled="disabled" />
    </ui-dropdown>
    <ui-button :submit="true" label="@ui.save" :state="state" v-if="!disabled" />
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
</style>
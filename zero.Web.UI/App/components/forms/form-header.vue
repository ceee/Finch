<template>
  <ui-header-bar class="ui-form-header" :back-button="true">
    <template v-slot:title>
      <h2 class="ui-header-bar-title" :class="{'is-empty': title && !value.name}">
        <input class="ui-form-header-title-input" type="text" v-model="value.name" v-localize:placeholder="title" :readonly="titleDisabled || disabled" />
        <!--<ui-alias class="ui-form-header-title-alias" v-if="hasAlias" v-model="value.alias" :name="value.name" :disabled="disabled" />-->
        <!--<span v-localize="value.name || title"></span>-->
      </h2>
    </template>
    <div class="ui-form-header-aside">
      <slot></slot>
      <div v-if="activeToggle" class="ui-form-header-toggle">
        <ui-toggle v-model="value.isActive" class="is-primary" off-content="@ui.inactive" :off-warning="true" on-content="@ui.active" :content-left="true" :disabled="disabled" />
      </div>
      <ui-dropdown v-if="actionsDefined && !disabled" align="right">
        <template v-slot:button>
          <ui-button type="light onbg" label="@ui.actions" caret="down" />
        </template>
        <slot name="actions"></slot>
        <ui-dropdown-button v-if="canDelete" label="@ui.delete" icon="fth-trash" @click="onDelete" :disabled="disabled" />
      </ui-dropdown>
      <ui-button :submit="true" type="primary" label="@ui.save" :state="state" v-if="!disabled" />
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
      titleDisabled: {
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
      },
      hasAlias: {
        type: Boolean,
        default: true
      }
    },

    computed: {
      actionsDefined()
      {
        return !this.isCreate && (this.canDelete || this.$scopedSlots.hasOwnProperty('actions'));
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
    display: flex;
    align-items: center;
    justify-content: flex-end;

    > * + *
    {
      margin-left: var(--padding-s);
    }
  }

  .ui-form-header-toggle
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    position: relative;
    top: -1px;
    margin-left: var(--padding-s);
    margin-right: var(--padding-s);

    .ui-toggle-switch, .ui-toggle-switch.is-active
    {
      background: var(--color-button-light-onbg);
      box-shadow: var(--shadow-short);

      i
      {
        background: var(--color-text);
      }
    }

    .ui-toggle-off-warning
    {
      color: var(--color-accent-red);
    }
  }

  .ui-header-bar-title
  {
    position: relative;
  }

  input[type="text"].ui-form-header-title-input
  {
    font-family: var(--font);
    color: var(--color-text);
    font-size: var(--font-size-l);
    font-weight: 700;
    background: none;
    border: 1px dashed var(--color-line-dashed);

    /*&:hover, &:focus, .ui-header-bar-title.is-empty &
    {
      border: 1px dashed var(--color-text-dim-one);
    }*/
  }

  .ui-form-header-title-alias
  {
    position: absolute;
    right: 10px;
    top: 11px;
    z-index: 2;

    .ui-alias-lock
    {
      background: none;
    }
  }
</style>
<template>
  <div class="ui-module-item" v-if="!loading" :data-module="alias">
    <button type="button" v-if="module" class="ui-module-item-header" @click="$emit('edit', module, value)"><i :class="module.icon"></i> {{module.name}}</button>
    
    <button v-if="!module" type="button" class="ui-module-item-header is-error" disabled><i class="fth-alert-circle"></i> {{alias}}</button>
    <p v-if="!module" class="ui-module-item-error">Could not find a registered module <code>[{{alias}}]</code> in the code.</p>

    <ui-module-preview-inner v-if="tryRender" :template="renderer.preview.template" :value="value" />

    <div v-if="!value.isActive" class="ui-module-item-disabled"></div>

    <div class="ui-module-item-actions">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-icon-button v-if="!value.isActive" class="ui-module-item-disabled-icon" icon="fth-lock" title="Disabled" />
          <ui-icon-button v-else icon="fth-more-horizontal" title="Actions" />
        </template>
        <ui-dropdown-button v-if="canEdit" label="Edit" icon="fth-edit-2" @click="$emit('edit', module, value)" />
        <ui-dropdown-button v-if="value.isActive" label="Disable" icon="fth-lock" @click="value.isActive = false" />
        <ui-dropdown-button v-else label="Enable" icon="fth-unlock" @click="value.isActive = true" />
        <ui-dropdown-button label="Remove" icon="fth-trash" @click="$emit('remove', module, value)" />
      </ui-dropdown>
    </div>
  </div>
</template>


<script>
  import { find as _find } from 'underscore';

  export default {
    name: 'uiModulePreview',

    props: {
      value: {
        type: Object,
        default: () => { }
      },
      types: {
        type: Array,
        default: () => []
      },
      config: Object
    },


    data: () => ({
      loading: true,
      module: {},
      renderer: {}
    }),


    watch: {
      value(val)
      {
        this.render(val);
      }
    },


    computed: {
      alias()
      {
        return this.value.moduleTypeAlias;
      },
      tryRender()
      {
        return this.module && this.renderer && this.renderer.preview && this.renderer.preview.template;
      },
      canEdit()
      {
        return this.module && this.renderer;
      }
    },


    mounted()
    {
      this.render(this.value);
    },


    methods: {

      render(value)
      {
        this.loading = true;
        this.module = _find(this.types, x => x.alias == this.alias);
        this.renderer = zero.renderers['module.' + this.alias];
        this.$nextTick(() => this.loading = false);
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-item
  {
    display: grid !important;
    grid-template-columns: 1fr auto;
    grid-template-rows: auto auto;
    grid-column-gap: var(--padding);
    position: relative;
  }

  .ui-module-item-header
  {
    grid-column: 1;
    grid-row: 1;
    display: flex;
    align-items: center;
    color: var(--color-fg-dim);
    font-size: var(--font-size-s);

    i
    {
      font-size: var(--font-size-l);
      margin-right: 10px;
      position: relative;
      top: -1px;
    }

    &.is-error
    {
      color: var(--color-accent-error);
    }
  }

  .ui-module-item-disabled
  {
    background: var(--color-bg-bright);
    opacity: .6;
    position: absolute;
    left: 0;
    right: 0;
    top: 0;
    bottom: 0;
    z-index: 1;
  }

  .ui-module-item-disabled-icon
  {
    position: relative;
    z-index: 1;

    .ui-button-icon
    {
      color: var(--color-accent-error);
    }
  }

  .ui-module-preview-inner
  {
    grid-column: 1;
    grid-row: 2;
    margin-top: 12px;
    max-width: 1060px;

    p
    {
      margin: 0;
    }

    p + p
    {
      margin-top: 10px;
    }

    a
    {
      color: var(--color-fg);
      text-decoration: underline dotted var(--color-fg-dim);
      text-underline-offset: 3px;
    }
  }

  .ui-module-item-actions
  {
    display: flex;
    grid-column: 2;
    grid-row: span 2 / auto;
    align-self: center;
    margin: -10px 0;

    > * + *
    {
      margin-left: 10px;
    }
  }

  .ui-module-item-error
  {
    margin: 0;
    grid-column: 1;
    grid-row: 2;
    margin-top: 12px;
  }
</style>
<template>
  <div class="ui-module-item" v-if="!loading" :data-module="alias" :class="{'can-edit': canEdit }">
    <div class="ui-module-item-content" v-if="module" @click="emit('edit')">
      <button type="button" class="ui-module-item-header"><i :class="module.icon"></i> {{module.name}}</button>
      <ui-module-preview-inner v-if="tryRender" :template="renderer.preview.template" :value="value" @click="emit('edit')" />
    </div>
    <div class="ui-module-item-content" v-else>
      <button type="button" class="ui-module-item-header is-error" disabled><i class="fth-alert-circle"></i> {{alias}}</button>
      <p class="ui-module-item-error" v-localize:html="{ key: '@modules.notfound', tokens: { alias: alias } }"></p>
    </div>

    <div v-if="!value.isActive" class="ui-module-item-disabled"></div>

    <div class="ui-module-item-actions">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-icon-button v-if="!value.isActive" class="ui-module-item-disabled-icon" icon="fth-lock" title="Disabled" />
          <ui-icon-button v-else icon="fth-more-horizontal" title="Actions" />
        </template>
        <ui-dropdown-button v-if="canEdit" label="Edit" icon="fth-edit-2" @click="emit('edit')" />
        <ui-dropdown-button v-if="value.isActive" label="Disable" icon="fth-lock" @click="toggleStatus(false)" />
        <ui-dropdown-button v-else label="Enable" icon="fth-unlock" @click="toggleStatus(true)" />
        <ui-dropdown-button label="Remove" icon="fth-trash" @click="emit('remove')" />
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
        return this.module && this.renderer && this.renderer.fields && this.renderer.fields.length > 0;
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
      },

      emit(ev)
      {
        if (!this.canEdit && ev === 'edit')
        {
          return;
        }
        this.$emit(ev, this.module, this.value);
      },

      toggleStatus(isActive)
      {
        this.value.isActive = isActive;
        this.emit('isActive');
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-item
  {
    display: grid !important;
    grid-template-columns: 1fr auto;
    grid-column-gap: var(--padding);
    position: relative;
    margin: 0 -32px;
    padding: var(--padding);
    /*margin-top: var(--padding);
    padding-top: var(--padding);*/
    border-bottom: 1px solid var(--color-line);

    &.can-edit .ui-module-item-content
    {
      cursor: pointer;
    }

    &:not(.can-edit) .ui-module-item-header
    {
      cursor: default;
    }

    /*&:first-child
    {
      border-top: none;
    }*/
  }

  .ui-module-item-header
  {
    display: flex;
    align-items: center;
    color: var(--color-text-dim);
    font-size: var(--font-size-s);
    width: 100%;

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
    background: var(--color-box);
    border-radius: var(--radius);
    opacity: .5;
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
    padding-top: 12px;
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
      color: var(--color-text);
      text-decoration: underline dotted var(--color-text-dim);
      text-underline-offset: 3px;
    }
  }

  .ui-module-item-actions
  {
    display: flex;
    grid-column: 2;
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
    margin-top: 12px;
  }
</style>
<template>
  <div class="ui-module-item" v-if="!loading" :data-module="alias" :class="{'can-edit': canEdit, 'no-preview': !preview }">
    <div class="ui-module-item-content" v-if="module" @click="emit('edit')">
      <span v-if="!preview || !preview.hideLabel" class="ui-module-item-header"><ui-icon :symbol="module.icon" /> <span v-localize="module.name"></span></span>
      <module-preview-inner v-if="tryRender && typeof preview.template === 'string'" :template="preview.template" :value="value" :options="preview" />
      <div v-if="tryRender && typeof preview.template !== 'string'" class="ui-module-preview-inner">
        <component :is="preview.template" :model="value" :options="preview" />
      </div>
    </div>
    <div class="ui-module-item-content" v-else>
      <span class="ui-module-item-header is-error"><ui-icon symbol="fth-alert-circle" /> {{alias}}</span>
      <p class="ui-module-item-error" v-localize:html="{ key: '@modules.notfound', tokens: { alias: alias } }"></p>
    </div>

    <div v-if="!value.isActive" class="ui-module-item-disabled"></div>

    <div class="ui-module-item-actions" v-if="!disabled">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-icon-button v-if="!value.isActive" class="ui-module-item-disabled-icon ui-module-item-action" icon="fth-lock" title="Disabled" />
          <ui-icon-button v-else icon="fth-more-vertical" class="ui-module-item-action" title="Actions" />
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
  import ModulePreviewInner from './preview-inner.vue';


  export default {
    name: 'uiModulePreview',

    components: { ModulePreviewInner },

    props: {
      value: {
        type: Object,
        default: () => { }
      },
      types: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    },

    data: () => ({
      loading: true,
      module: {},
      renderer: {},
      preview: null
    }),

    watch: {
      value: {
        deep: true,
        handler(val, oldVal)
        {
          // TODO this sucks
          // but we do always get changes for ALL modules when anything changes
          // this triggers reload of all modules which can be super expensive
          if (JSON.stringify(val) !== JSON.stringify(oldVal))
          {
            this.render(val);
          }
        }
      }
    },

    computed: {
      alias()
      {
        return this.value.moduleTypeAlias;
      },
      tryRender()
      {
        return this.preview && this.preview.template;
      },
      canEdit()
      {
        return true; //this.module && this.renderer && this.renderer.fields && this.renderer.fields.length > 0;
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
        this.module = this.types.find(x => x.alias == this.alias);
        //this.renderer = this.zero.getEditor('module.' + this.alias);
        //if (this.renderer)
        //{
        //  this.preview = this.renderer.previewOptions;
        //}
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
    grid-template-columns: 1fr auto 0;
    grid-column-gap: var(--padding);
    position: relative;
    margin: 0 -32px;
    margin-right: -96px;
    border-right: 3px solid transparent;
    padding: 0; //var(--padding);
    /*margin-top: var(--padding);
    padding-top: var(--padding);*/
    //border-bottom: 1px dashed var(--color-line-dashed);
    &.can-edit .ui-module-item-content
    {
      cursor: pointer;
    }

    &:not(.can-edit) .ui-module-item-header
    {
      cursor: default;
    }
    /*&:hover
    {
      border-right: 3px solid var(--color-line-onbg);
    }*/
    /*&:first-child
    {
      border-top: none;
    }*/
  }

  .ui-module-item-content
  {
    padding: var(--padding-m) var(--padding);
    //padding-left: calc(var(--padding) * 3);
    padding-right: 0;
  }

  .ui-module-item-header
  {
    display: flex; //TODO flex;
    align-items: center;
    color: var(--color-text-dim-one);
    font-size: var(--font-size-xs);
    line-height: 1.5;
    width: 100%;

    .ui-icon
    {
      font-size: var(--font-size-l);
      margin-right: 10px;
    }

    &.is-error
    {
      color: var(--color-accent-error);
    }

    .no-preview &
    {
      color: var(--color-text);
      font-size: var(--font-size);
      font-weight: 700;
    }
  }

  .ui-module-item-disabled
  {
    background: var(--color-box);
    border-radius: var(--radius);
    opacity: .5;
    position: absolute;
    left: 0;
    right: 65px;
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
    max-width: 1060px;

    .ui-module-item-header + &
    {
      padding-top: 4px;
    }

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
    position: relative;
    right: 0;
    //opacity: 0;
    transition: opacity .2s ease;

    > * + *
    {
      margin-left: 10px;
    }

    .ui-module-item:hover &
    {
      opacity: 1;
    }
  }

  .ui-icon-button.ui-module-item-action
  {
    background: var(--color-box);
    height: 50px;
    width: 24px;
    border-radius: var(--radius-inner);

    &.ui-module-item-disabled-icon .ui-icon
    {
      margin-left: -6px;
    }
  }

  .ui-module-item-error
  {
    margin: 0;
    grid-column: 1;
    margin-top: 12px;
  }
</style>
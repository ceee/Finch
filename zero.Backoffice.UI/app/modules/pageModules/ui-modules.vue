<template>
  <div v-if="loaded" class="ui-modules-inner">
    <div v-if="items.length" class="ui-modules-inner-sortable" v-sortable="{ onUpdate: onSortingUpdated }">
      <module-preview v-for="item in items" :key="item.id" :types="moduleTypes" :value="item" @edit="edit" @remove="remove" @isActive="onChange" :disabled="disabled" />
    </div>
    <button v-if="canAdd" type="button" class="ui-modules-start-button" @click="selectModule">
      <span class="ui-modules-start-button-icon"><ui-icon symbol="fth-plus" :size="19" /></span>
      <p class="ui-modules-start-button-text"><strong>Add content</strong></p>
    </button>
  </div>
</template>


<script lang="ts">
  import api from './api';
  import { arrayMove, arrayRemove, arrayReplace } from '../../utils';
  import * as overlays from '../../services/overlay';
  import ModulePreview from './partials/preview.vue';

  export default {
    name: 'uiModules',

    props: {
      value: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      },
      tags: {
        type: Array,
        default: () => []
      },
      config: Object
    },


    components: { ModulePreview },


    data: () => ({
      loaded: false,
      items: [],
      moduleTypes: []
    }),


    watch: {
      value(val)
      {
        this.setup(val);
      }
    },


    computed: {
      canAdd()
      {
        return !this.disabled;
      }
    },


    created()
    {
      api.getTypes(undefined, this.tags).then(res =>
      {
        this.moduleTypes = res.data;
        this.setup(this.value);
        this.loaded = true;
      });
    },


    methods: {

      setup(value)
      {
        this.items = JSON.parse(JSON.stringify(value || []));
      },


      selectModule()
      {
        overlays.open({
          alias: 'modules-select',
          component: ModuleSelectOverlay,
          types: this.moduleTypes,
          width: null
        }).then(module => this.onAdd(module), () => { });
      },


      onAdd(module)
      {
        this.edit(module, null, true);
      },


      onSortingUpdated(ev)
      {
        this.items = arrayMove(this.items, ev.oldIndex, ev.newIndex);
        let sort = 0;
        this.items.forEach(x => x.sort = sort++);
        this.onChange();
      },


      edit(module, model, isAdd)
      {
        const alias = 'module.' + module.alias;
        const editor = this.zero.getEditor(alias);

        if (!editor)
        {
          // TODO throw error
        }

        // some modules can have no fields for editing, so we add them directly
        if (!editor.fields || editor.fields.length < 1)
        {
          return api.getEmpty(module.alias).then(res =>
          {
            this.items.push(res.entity);
            this.onChange();
          });
        }

        // open editing overlay
        return overlays.open({
          component: EditModuleOverlay,
          display: 'editor',
          module: module,
          editor: editor,
          model: model,
          width: 820
        }).then(value =>
        {
          if (isAdd)
          {
            this.items.push(value);
          }
          else
          {
            arrayReplace(this.items, model, value);
          }

          this.onChange();
        });
      },


      remove(module, model)
      {
        arrayRemove(this.items, model);
        this.onChange();
      },


      onChange()
      {
        this.$emit('input', this.items);
      }
    }
  }
</script>

<style lang="scss">

  .ui-modules-inner
  {
    margin: var(--padding-m);
    margin-top: -16px;
  }

  .ui-modules-inner-sortable
  {
    padding: var(--padding-s) 0;
  }

  .ui-modules-start-button
  {
    color: var(--color-primary);
    font-size: var(--font-size);
    display: inline-grid;
    grid-template-columns: auto 1fr;
    gap: 25px;
    align-items: center;
    margin-top: var(--padding);
    margin-bottom: -10px;
  }

  .ui-modules-inner-sortable + .ui-modules-start-button
  {
    width: 100%;
    padding-top: var(--padding);
    margin-top: var(--padding-xs);
    border-top: 1px dashed var(--color-line-dashed-onbg);
  }

  .ui-modules-start-button-icon
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    width: 52px;
    height: 52px;
    background: var(--color-button-light);
    border-radius: var(--radius);
  }

  .ui-modules-start-button-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);
    margin: 0;
    font-size: var(--font-size-s);

    strong
    {
      display: inline-block;
      margin-bottom: 2px;
      color: var(--color-text);
      font-size: var(--font-size);
    }
  }
</style>
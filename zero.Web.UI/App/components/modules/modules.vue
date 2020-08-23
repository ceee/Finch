<template>
  <div class="ui-modules-inner">
    <ui-module-preview v-for="item in items" :key="item.id" :types="moduleTypes" :value="item" @edit="edit" @remove="remove" />
    <ui-modules-select ref="moduleSelect" :types="moduleTypes" :value="value" v-if="canAdd" @selected="onAdd" />
  </div>
</template>


<script>
  import ModulesApi from 'zero/resources/modules.js';
  import EditModuleOverlay from './edit-module';
  import Overlay from 'zero/services/overlay.js';
  import Arrays from 'zero/services/arrays.js';

  export default {
    name: 'uiModules',

    props: {
      value: {
        type: Array,
        default: () => []
      },
      config: Object
    },


    data: () => ({
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
        return true;
      }
    },


    created()
    {
      ModulesApi.getModuleTypes().then(res =>
      {
        this.moduleTypes = res;
        this.setup(this.value);
      });
    },


    methods: {

      setup(value)
      {
        this.items = JSON.parse(JSON.stringify(value || []));
      },


      onAdd(module)
      {
        this.edit(module, null, true);
      },


      edit(module, model, isAdd)
      {
        return Overlay.open({
          component: EditModuleOverlay,
          display: 'editor',
          module: module,
          renderer: 'module.' + module.alias,
          model: model,
          width: 1100
        }).then(value =>
        {
          if (isAdd)
          {
            this.items.push(value);
            this.$refs.moduleSelect.reset();
          }
          else
          {
            Arrays.replace(this.items, model, value);
          }

          this.onChange();
        });
      },


      remove(module, model)
      {
        Arrays.remove(this.items, model);
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
  @import './Sass/Modules/box';

  .ui-property.ui-modules
  {
    width: 100%;
    margin: 0;
    margin-top: var(--padding);
  }

  .ui-modules-start, .ui-module-item
  {
    margin: 0;
    padding: var(--padding);
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    box-shadow: var(--color-shadow-short);
    display: flex;
    justify-content: center;
  }

  .ui-module-item
  {
    display: block;
  }

  .ui-module-item + .ui-module-item,
  .ui-module-item + .ui-modules-start
  {
    margin-top: 3px;
  }

  .ui-modules-start-button
  {
    color: var(--color-fg);
    font-size: var(--font-size);
    display: inline-grid;
    grid-template-columns: auto 1fr;
    grid-gap: 25px;
    align-items: center;
  }

  .ui-modules-start-button-icon
  {
    width: 70px;
    height: 70px;
    line-height: 68px !important;
    font-size: 20px;
    text-align: center;
    background: var(--color-bg-bright-two);
    border-radius: var(--radius);
  }

  .ui-modules-start-button-text
  {
    line-height: 1.3;
    color: var(--color-fg-dim);

    strong
    {
      display: inline-block;
      margin-bottom: 5px;
      color: var(--color-fg);
    }
  }

  .ui-module-item-header
  {
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
  }
</style>
<template>
  <div class="ui-modules-inner">
    <div class="ui-module-item" v-for="item in items">
      <header class="ui-module-item-header"><i :class="getModule(item.moduleTypeAlias).icon"></i> {{getModule(item.moduleTypeAlias).name}}</header>
    </div>
    <ui-modules-select :types="moduleTypes" :value="value" v-if="canAdd" @selected="onSelected" />
  </div>
</template>


<script>
  import ModulesApi from 'zero/resources/modules.js';
  import EditModuleOverlay from './edit-module';
  import Overlay from 'zero/services/overlay.js';
  import { find as _find } from 'underscore';

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
      canAdd: true,
      moduleTypes: []
    }),


    watch: {
      value(val)
      {
        this.setup(val);
      }
    },


    created()
    {
      this.setup(this.value);

      ModulesApi.getModuleTypes().then(res =>
      {
        this.moduleTypes = res;
      });
    },


    methods: {

      setup(value)
      {
        this.items = JSON.parse(JSON.stringify(value || []));
      },


      getModule(alias)
      {
        return _find(this.moduleTypes, x => x.alias == alias);
      },


      onSelected(module, isAdd)
      {
        return Overlay.open({
          component: EditModuleOverlay,
          display: 'editor',
          module: module,
          renderer: 'module.' + module.alias,
          model: {},
          width: 1100
        }).then(value =>
        {
          this.items.push(value);
          this.onChange();
        });
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
</style>
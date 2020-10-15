<template>
  <div class="ui-modules-inner">
    <div v-if="items.length" class="ui-modules-inner-sortable" v-sortable="{ onUpdate: onSortingUpdated }">
      <ui-module-preview v-for="item in items" :key="item.id" :types="moduleTypes" :value="item" @edit="edit" @remove="remove" :disabled="disabled" />
    </div>
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

    emits: ['input'],

    props: {
      value: {
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


      onSortingUpdated(ev)
      {
        this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
        let sort = 0;
        this.items.forEach(x => x.sort = sort++);
        this.onChange();
      },


      edit(module, model, isAdd)
      {
        const alias = 'module.' + module.alias;
        let renderer = zero.renderers[alias];

        // some modules can have no fields for editing, so we add them directly
        if (!renderer.fields || renderer.fields.length < 1)
        {
          return ModulesApi.getEmpty(module.alias).then(res =>
          {
            this.items.push(res.entity);
            this.$refs.moduleSelect.reset();
            this.onChange();
          });
        }

        // open editing overlay
        return Overlay.open({
          component: EditModuleOverlay,
          display: 'editor',
          module: module,
          renderer: alias,
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
  .ui-modules-inner-sortable
  {
    margin-top: -32px;
  }
</style>
<template>
  <div class="ui-module-item" v-if="!loading">
    <header class="ui-module-item-header"><i :class="module.icon"></i> {{module.name}}</header>
    <ui-module-preview-inner :template="renderer.preview.template" :value="value" />
    <!--<component :is="component" :module="module" />-->
  </div>
</template>


<script>
  import Vue from 'vue';
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
      renderer: {},
      component: null
    }),


    watch: {
      value(val)
      {
        this.render(val);
      }
    },


    components: {
      'preview': (a,b,c) =>
      {
        console.info(a,b,c);
        return {

        }
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

        this.module = _find(this.types, x => x.alias == this.value.moduleTypeAlias);
        this.renderer = zero.renderers['module.' + this.value.moduleTypeAlias];

        this.loading = false;
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-item
  {

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
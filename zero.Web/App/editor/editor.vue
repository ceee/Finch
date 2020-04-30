<template>
  <component v-if="loaded" :is="rootNode" class="editor">
    <ui-tab v-if="hasTabs" class="ui-box" :label="component.params.name" v-for="component in components">
      <editor-component v-for="child in component.components" v-model="model[child.params.field]" :component="child" />
    </ui-tab>
    <editor-component v-if="!hasTabs" v-for="component in components" v-model="model[component.params.field]" :component="component" />
  </component>
</template>


<script>
  import Axios from 'axios';
  import EditorComponent from 'zero/editor/editor-component';

  export default {
    name: 'uiEditor',

    components: { EditorComponent },

    data: () => ({
      loaded: false,
      hasTabs: false,
      model: {},
      components: []
    }),

    computed: {
      rootNode()
      {
        return this.hasTabs ? 'ui-tabs' : 'div';
      }
    },

    watch: {
      model: {
        deep: true,
        handler: function()
        {
          console.info('change:editor')
          console.table(JSON.parse(JSON.stringify(this.model)));
        }
      }
    },  

    created()
    {
      Axios.get('test/renderConfig').then(res =>
      {
        this.components = res.data.components;
        this.hasTabs = this.components.length > 0 && this.components[0].method === 'tab';
        this.loaded = true;
        console.info(JSON.parse(JSON.stringify(this.components)));
      });
    }
  }
</script>
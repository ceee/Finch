<template>
  <component v-if="loaded" :is="rootNode" class="editor">
    <ui-tab class="ui-box" v-if="hasTabs" :label="component.params.name" v-for="component in components">
       <editor-component v-for="child in component.components" :component="child" />
    </ui-tab>
    <editor-component v-if="!hasTabs" v-for="component in components" :component="component" />
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
      components: []
    }),

    computed: {
      rootNode()
      {
        return this.hasTabs ? 'ui-tabs' : 'div';
      }
    },

    created()
    {
      Axios.get('test/renderConfig').then(res =>
      {
        this.components = res.data.components;
        this.hasTabs = this.components.length > 0 && this.components[0].method === 'tab';
        this.loaded = true;
        console.dir(JSON.parse(JSON.stringify(res.data.components)));
      });
    }
  }
</script>
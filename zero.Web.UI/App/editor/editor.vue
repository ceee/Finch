<template>
  <component v-if="loaded" :is="rootNode" class="editor">
    <ui-tab v-if="hasTabs" class="ui-box" :label="component.params.name" v-for="(component, index) in components" :key="index">
      <editor-component v-for="(child, index) in component.components" :key="index" v-model="model[child.params.field]" :component="child" />
    </ui-tab>
    <div v-if="!hasTabs" class="ui-box">
      <editor-component v-for="(component, index) in components" :key="index" v-model="model[component.params.field]" :component="component" />
    </div>
  </component>
</template>


<script>
  import Axios from 'axios';
  import EditorComponent from 'zero/editor/editor-component';

  export default {
    name: 'uiEditor',

    props: {
      config: {
        type: Object,
        required: true
      },
      value: {
        type: Object
      }
    },

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
      config: {
        deep: true,
        handler: function()
        {
          this.load();
        }
      },
      value: {
        deep: true,
        handler: function (value)
        {
          this.model = value;
        }
      },
      model: {
        deep: true,
        handler: function()
        {
          //console.info('change:editor')
          //console.table(JSON.parse(JSON.stringify(this.model)));
        }
      }
    },  

    created()
    {
      this.load(); 
    },

    methods: {

      load()
      {
        if (!this.config.components)
        {
          return;
        }
        this.components = this.config.components;
        this.hasTabs = this.components.length > 0 && this.components[0].method === 'tab';
        this.loaded = true;
      }
    }
  }
</script>
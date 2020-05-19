<template>
  <component v-if="loaded" :is="rootNode" class="editor">
    <ui-tab v-if="hasTabs" class="ui-box" :label="component.params.name" v-for="(component, index) in components" :key="index">
      <editor-component v-for="(child, index) in component.components" :key="index" :field="child.params.field" v-model="value" :component="child" />
    </ui-tab>
    <div v-if="!hasTabs" class="ui-box">
      <editor-component v-for="(component, index) in components" :key="index" :field="component.params.field" v-model="value" :component="component" />
    </div>
  </component>
</template>


<script>
  import EditorComponent from 'zero/editor/editor-component';
  import RendererApi from 'zero/resources/renderer';

  export default {
    name: 'uiEditor',

    props: {
      config: {
        type: [ String, Object ],
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
      }
    },  

    created()
    {
      this.load();
    },

    methods: {

      load()
      {
        if (typeof this.config === 'string')
        {
          RendererApi.getByAlias(this.config).then(response =>
          {
            this.finishLoad(response);
          });
        }
        else
        {
          this.finishLoad(this.config);
        }
      },

      finishLoad(config)
      {
        if (!config.components)
        {
          return;
        }

        this.components = config.components;
        this.hasTabs = this.components.length > 0 && this.components[0].method === 'tab';
        this.loaded = true;
      }
    }
  }
</script>
<template>
  <div class="editor" v-if="loaded" :class="['display-' + display, { 'has-sidebar': asideDefined, 'hide-tabs': tabs.length < 2 }]">
    <ui-tabs class="editor-tabs">
      <ui-tab v-if="!tab.disabled(value)" v-for="(tab, index) in tabs" class="ui-box" :class="tab.class" :label="tab.name" :count="tab.count(value)" :key="index">
        <h3 v-if="display == 'boxes'" class="ui-headline" v-localize="tab.name"></h3>
        <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" @input="onChange" :editor="editorConfig" :value="value" :class="field.options.class" />
        <component v-if="tab.component" :is="tab.component" v-model="value" />
      </ui-tab>
    </ui-tabs>
    <aside class="editor-aside" v-if="asideDefined">
      <slot name="aside"></slot>
    </aside>
    <aside class="editor-below" v-if="belowDefined">
      <slot name="below"></slot>
    </aside>
  </div>
</template>


<script>
  import './editor.scss';
  import EditorComponent from 'zero/editor/editor-component.vue';
  import EditorAside from './editor-aside.vue';

  export default {
    name: 'uiEditor',

    provide: function ()
    {
      return {
        meta: this.meta,
        editor: this.config
      };
    },

    props: {
      config: {
        type: [String, Object],
        required: true
      },
      meta: {
        type: Object,
        default: () => { }
      },
      value: {
        type: Object
      },
      onConfigure: {
        type: Function,
        default: () => { }
      },
    },

    components: { EditorComponent, EditorAside },

    data: () => ({
      editorConfig: {},
      loaded: false,
      tabs: []
    }),

    computed: {
      asideDefined()
      {
        return this.$scopedSlots.hasOwnProperty('aside');
      },
      belowDefined()
      {
        return this.$scopedSlots.hasOwnProperty('below');
      }
    },

    created()
    {
      this.editorConfig = typeof this.config === 'string' ? this.zero.getEditor(this.config) : this.config;

      this.tabs = this.editorConfig.tabs.map(tab =>
      {
        return {
          ...tab,
          count: value => typeof tab.count === 'number' ? tab.count : (typeof tab.count === 'function' ? tab.count(value) : 0),
          disabled: value => typeof tab.disabled === 'boolean' ? tab.disabled : (typeof tab.disabled === 'function' ? tab.disabled(value) : false),
          fields: this.editorConfig.getFields(tab)
        };
      });

      this.display = this.editorConfig.options.display || 'tabs';
      this.onConfigure(this);

      this.loaded = true;
    },

    methods: {

      onChange()
      {
        this.$emit('input', this.value);
      }
    }
  }
</script>
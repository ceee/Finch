<template>
  <div>
    <div class="editor-outer" v-if="loaded" :class="{ 'has-tabs': hasTabs, '-infos-aside': !nested, 'is-page': isPage }">
      <ui-tabs class="editor">
        <ui-tab v-if="!tab.disabled(value)" v-for="(tab, index) in tabs" class="ui-box" :class="tab.class" :label="tab.name" :count="tab.count(value)" :key="index">
          <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" @input="onChange" :editor="editorConfig" :value="value" />
          <component v-if="tab.component" :is="tab.component" v-model="value" />
        </ui-tab>
      </ui-tabs>
      <editor-aside :editor="editorConfig" :value="value" v-bind="{ infos, activeToggle, nested, isPage }" />
    </div>
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
        disabled: false
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
      infos: {
        type: String,
        default: 'aside'
      },
      activeToggle: {
        type: Boolean,
        default: true
      },
      nested: {
        type: Boolean,
        default: false
      },
      isPage: {
        type: Boolean,
        default: false
      },
      onConfigure: {
        type: Function,
        default: () => { }
      },
    },

    components: { EditorComponent, EditorAside },

    data: () => ({
      editorConfig: null,
      loaded: false,
      hasTabs: false,
      tabs: []
    }),

    created()
    {
      this.editorConfig = typeof this.config === 'string' ? this.zero.getEditor(this.config) : this.config;

      if (this.editorConfig.tabs.length > 0)
      {
        this.hasTabs = true;
        this.tabs = this.editorConfig.tabs.map(tab =>
        {
          return {
            ...tab,
            count: value => typeof tab.count === 'number' ? tab.count : (typeof tab.count === 'function' ? tab.count(value) : 0),
            disabled: value => typeof tab.disabled === 'boolean' ? tab.disabled : (typeof tab.disabled === 'function' ? tab.disabled(value) : false),
            fields: this.editorConfig.getFields(tab)
          };
        });
      }
      else
      {
        this.tabs = [{
          name: '_',
          count: value => 0,
          disabled: value => false,
          fields: this.editorConfig.getFields()
        }];
      }

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
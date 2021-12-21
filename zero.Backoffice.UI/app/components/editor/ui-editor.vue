<template>
  <div class="editor-outer" v-if="loaded">
    <header class="editor-above" v-if="aboveDefined">
      <!--<slot name="above" v-bind:config="editorConfig"></slot>-->
    </header>
    <div class="editor" :class="['display-' + display, { 'has-sidebar': asideDefined, 'hide-tabs': editorConfig.tabs.length < 2, 'has-below': belowDefined }]">
      <ui-tabs class="editor-tabs">
        <ui-tab v-for="(tab, index) in editorConfig.tabs" :key="index" class="ui-box" 
                :class="tab.class" 
                :data-alias="tab.alias" 
                :label="tab.name"
                :count="tab.count(value)" 
                :hide="tab.hidden(value)"
                :disabled="tab.disabled(value)">
          <h3 v-if="display == 'boxes' && tab.name" class="ui-headline editor-tab-headline" v-localize="tab.name"></h3>
          <slot name="blueprint">
            <!--<blueprint-property v-if="value && editorConfig.blueprint" :value="value" :meta="meta" :config="editorConfig.blueprint" />-->
          </slot>
          <div v-if="!tab.component" class="ui-property ui-property-parent" v-for="(fieldset, fieldsetIndex) in tab.fieldsets" :key="fieldsetIndex">

            <editor-component v-for="(field, fieldIndex) in fieldset.fields" :key="fieldIndex"
                              :value="value"
                              :field="field"
                              :disabled="disabled" 
                              :system="system"
                              :class="field.configuration.classes"
                              @input="onChange" />

          </div>
          <component v-else :is="tab.component" v-model="value" :system="system" />
        </ui-tab>
      </ui-tabs>
      <aside class="editor-aside" v-if="asideDefined">
        <slot name="aside"></slot>
      </aside>
      <aside class="editor-below" v-if="belowDefined">
        <slot name="below"></slot>
      </aside>
    </div>
  </div>
</template>


<script>
  // :data-cols="!!field.options.fieldset" 
  // :style="{ 'grid-column': field.options.fieldset ? 'span ' + field.options.fieldsetColumns : null }"

  import './ui-editor.scss';
  import EditorComponent from './ui-editor-component.vue';
  //import { createBlueprintConfig } from './editor-blueprint';
  //import BlueprintProperty from './blueprint/property.vue';
  import { defineComponent } from 'vue';
  import { compileEditor } from '../../editor/compile';

  let createBlueprintConfig = () => null;

  export default defineComponent({
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
      disabled: {
        type: Boolean,
        default: false
      },
      scope: {
        type: Boolean,
        default: true
      },
      value: {
        type: Object
      },
      onConfigure: {
        type: Function,
        default: () => { }
      },
    },

    components: { EditorComponent },

    data: () => ({
      display: 'tabs',
      editorConfig: {},
      loaded: false,
      tabs: [],
      system: false,
      currentFieldset: null
    }),

    computed: {
      aboveDefined()
      {
        return this.$slots.hasOwnProperty('above');
      },
      asideDefined()
      {
        return this.$slots.hasOwnProperty('aside');
      },
      belowDefined()
      {
        return this.$slots.hasOwnProperty('below');
      }
    },

    async created()
    {
      this.system = this.$route.query.scope == 'system';
      const schema = typeof this.config === 'string' ? await this.zero.getSchema(this.config) : this.config;
      const editor = compileEditor(this.zero, schema);

      this.editorConfig = editor;
      //this.editorConfig.blueprint = createBlueprintConfig(this.zero, this.editorConfig, this.value);

      this.onConfigure(this);

      this.loaded = true;
    },

    methods: {

      onChange()
      {
        this.$emit('input', this.value);
      }
    }
  })
</script>
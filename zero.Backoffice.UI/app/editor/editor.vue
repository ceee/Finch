<template>
  <div class="editor-outer" v-if="loaded">
    <header class="editor-above" v-if="aboveDefined">
      <slot name="above" v-bind:config="editorConfig"></slot>
    </header>
    <div class="editor" :class="['display-' + display, { 'has-sidebar': asideDefined, 'hide-tabs': tabs.length < 2, 'has-below': belowDefined }]">
      <ui-tabs class="editor-tabs">
        <ui-tab v-for="(tab, index) in tabs" class="ui-box" :class="tab.class" :data-alias="tab.alias" :label="tab.name" :count="tab.count(value)" :hidden="tab.disabled(value)" :key="index">
          <h3 v-if="display == 'boxes' && tab.name" class="ui-headline editor-tab-headline" v-localize="tab.name"></h3>
          <slot name="blueprint">
            <!--<blueprint-property v-if="value && editorConfig.blueprint" :model-value="value" :meta="meta" :config="editorConfig.blueprint" />-->
          </slot>
          <div class="ui-property ui-property-parent" v-for="fieldset in tab.fieldsets">
            <editor-component v-for="(field, fieldIndex) in fieldset.fields" :disabled="disabled" :key="fieldIndex" :config="field" @input="onChange" :editor="editorConfig" :value="value"
                              :class="field.options.class" :data-cols="!!field.options.fieldset" :style="{ 'grid-column': field.options.fieldset ? 'span ' + field.options.fieldsetColumns : null }" />

          </div>
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
  </div>
</template>


<script>
  import './editor.scss';
  import EditorComponent from './editor-component.vue';
  import EditorAside from './editor-aside.vue';
  import { createBlueprintConfig } from './editor-blueprint';
  import BlueprintProperty from './blueprint/property.vue';
  import { defineComponent } from 'vue';

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
      value: {
        type: Object
      },
      onConfigure: {
        type: Function,
        default: () => { }
      },
    },

    components: { EditorComponent, EditorAside, BlueprintProperty },

    data: () => ({
      editorConfig: {},
      loaded: false,
      tabs: [],
      currentFieldset: null
    }),

    computed: {
      aboveDefined()
      {
        return this.$scopedSlots.hasOwnProperty('above');
      },
      asideDefined()
      {
        return this.$scopedSlots.hasOwnProperty('aside');
      },
      belowDefined()
      {
        return this.$scopedSlots.hasOwnProperty('below');
      }
    },

    async created()
    {
      this.editorConfig = typeof this.config === 'string' ? await this.zero.getSchema(this.config) : this.config;
      this.editorConfig.blueprint = null; //createBlueprintConfig(this.zero, this.editorConfig, this.value);

      this.tabs = this.editorConfig.tabs.map(tab =>
      {
        let fieldsets = this.editorConfig.getFieldsets(tab);

        return {
          ...tab,
          count: value => typeof tab.count === 'number' ? tab.count : (typeof tab.count === 'function' ? tab.count(value) : 0),
          disabled: value => typeof tab.disabled === 'boolean' ? tab.disabled : (typeof tab.disabled === 'function' ? tab.disabled(value) : false),
          fieldsets
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
  })
</script>
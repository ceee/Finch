<template>
  <div class="editor-outer" v-if="loaded && editorConfig">
    <header class="editor-above" v-if="aboveDefined">
      <!--<slot name="above" v-bind:config="editorConfig"></slot>-->
    </header>
    <div class="editor" :class="['display-' + editorConfig.display, { 'has-sidebar': asideDefined, 'hide-tabs': editorConfig.tabs.length < 2, 'has-below': belowDefined }]">
      <ui-tabs class="editor-tabs">
        <ui-tab v-for="(tab, index) in editorConfig.tabs" :key="index" class="ui-box" 
                :class="tab.class" 
                :data-alias="tab.alias" 
                :label="tab.name"
                :count="tab.count(value)" 
                :hide="tab.hidden(value)"
                :disabled="tab.disabled(value)">
          <h3 v-if="editorConfig.display == 'boxes' && tab.name" class="ui-headline editor-tab-headline" v-localize="tab.name"></h3>
          <slot name="blueprint">
            <blueprint-property v-if="value && editorConfig.blueprint.isEnabled" :value="value" :meta="meta" :config="editorConfig.blueprint" />
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
  <div class="editor-outer" v-if="loaded && !editorConfig">
    <div class="page page-error" style="min-height: 400px;">
      <ui-icon symbol="fth-cloud-snow" class="page-error-icon" :size="82" />
      <p class="page-error-text">
        <strong class="page-error-headline">Could not find editor</strong><br>
        <span v-if="typeof config === 'string'">Please register an editor schema with the alias:<br />[{{config}}]</span>
      </p>
    </div>
  </div>
</template>


<script>
  // :data-cols="!!field.options.fieldset" 
  // :style="{ 'grid-column': field.options.fieldset ? 'span ' + field.options.fieldsetColumns : null }"

  import './ui-editor.scss';
  import EditorComponent from './ui-editor-component.vue';
  import BlueprintProperty from './blueprint/property.vue';
  import { defineComponent } from 'vue';
  import { compileEditor } from './compile';

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

    components: { EditorComponent, BlueprintProperty },

    data: () => ({
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
      this.system = this.$route.query['zero.scope'] == 'system';
      const schema = typeof this.config === 'string' ? await this.zero.getSchema(this.config) : this.config;
      this.editorConfig = compileEditor(this.zero, schema);

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
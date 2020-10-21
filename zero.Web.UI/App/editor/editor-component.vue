<template>
  <ui-property v-if="!isHidden" :field="config.path" :label="label" :hide-label="config.options.hideLabel" :description="description" :required="isRequired">
    <component :is="config.component" v-bind="config.componentOptions" :value="model" :entity="value" @input="onChange" />
    <p v-if="config.options.helpText" class="ui-property-help" v-localize="config.options.helpText"></p>
  </ui-property>
</template>


<script>
  import Strings from 'zero/services/strings';
  import Objects from 'zero/services/objects';
  import Editor from 'zero/core/editor.js';
  import EditorField from 'zero/core/editor-field.js';
  import Localization from 'zero/services/localization';

  export default {
    name: 'uiEditorComponent',

    inject: [ 'meta', 'disabled' ],

    props: {
      config: {
        type: EditorField,
        required: true
      },
      editor: {
        type: Editor,
        required: true
      },
      value: {
        type: Object,
        required: true
      }
    },

    watch: {
      value: {
        deep: true,
        handler: function ()
        {
          this.rebuildModel();
        }
      },
      config: {
        deep: true,
        handler: function ()
        {
          this.rebuildConfig();
        }
      },
      editor: {
        deep: true,
        handler: function ()
        {
          this.rebuildConfig();
        }
      }
    },

    data: () => ({
      model: null
    }),

    created()
    {
      this.rebuildModel();
      this.rebuildConfig();
    },

    mounted()
    {
      this.rebuildModel();
    },

    computed: {
      isHidden()
      {
        return typeof this.config.options.condition === 'function' && !this.config.options.condition(this.value);
      },
      isRequired()
      {
        return typeof this.config.isRequired === 'function' ? this.config.isRequired(this.value) : this.config.isRequired;
      },
      isDisabled()
      {
        return this.disabled || (typeof this.config.options.disabled === 'boolean' && this.config.options.disabled) || (typeof this.config.options.disabled === 'function' && this.config.options.disabled(this.value));
      },
      label()
      {
        return this.config.options.label || this.editor.templateLabel(this.config.path);
      },
      description()
      {
        return Localization.localize(this.config.options.description || this.editor.templateDescription(this.config.path), { hideEmpty: true });
      }
    },

    methods: {

      rebuildModel()
      {
        this.selector = Strings.selectorToArray(this.config.path);
        let currentValue = this.value;
        let found = false;

        if (!this.selector || !this.selector.length || !currentValue)
        {
          found = true;
          this.model = null;
        }
        else
        {
          for (var key of this.selector)
          {
            if (key in currentValue)
            {
              found = true;
              currentValue = currentValue[key];
            }
            else
            {
              break;
            }
          }

          this.model = found ? currentValue : null;
        }
      },


      rebuildConfig()
      {
        // build class list
        //let classes = typeof this.config.class === 'string' ? this.config.class.split(' ') : (this.config.class || []);
        //if (this.view === 'renderer' || this.view === 'nested')
        //{
        //  classes.push('full-width');
        //}
        //if (this.depth > 0)
        //{
        //  classes.push('is-nested');
        //}
        //if (this.isStatic)
        //{
        //  classes.push('is-static');
        //}
        //this.classList = classes;


        //// build label
        //let label = null;
        //if (this.config.label && this.config.label.indexOf('@') === 0)
        //{
        //  label = this.config.label;
        //}
        //else if (this.renderer.labelTemplate)
        //{
        //  label = this.renderer.labelTemplate(this.config.label || this.config.field);
        //}
        //else
        //{
        //  label = this.config.label || this.config.field;
        //}
        //this.label = Localization.localize(label);


        //// build description
        //let description = null;
        //if (this.config.description && this.config.description.indexOf('@') === 0)
        //{
        //  description = this.config.description;
        //}
        //else if (this.config.description === null)
        //{
        //  description = null;
        //}
        //else if (this.renderer.descriptionTemplate)
        //{
        //  description = this.renderer.descriptionTemplate(this.config.description || this.config.field);
        //}
        //else
        //{
        //  description = this.config.description;
        //}
        //this.description = Localization.localize(description, { hideEmpty: true });
      },


      onChange(value)
      {
        if (typeof value === 'function')
        {
          value(this.value);
        }
        else
        {
          Objects.setValue(this.value, this.selector, value);
        }
        this.$emit('input', this.value);
      }
    }
  }
</script>
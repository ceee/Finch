<template>
  <ui-property v-if="fulfillsCondition" :field="config.field" :label="label" :hide-label="config.hideLabel" :description="description" :required="config.required" :class="classList" :is-text="view === 'output'">
    <component v-if="fieldComponent" :is="fieldComponent" :config="config" :value="model" :entity="value" @input="onChange" :meta="meta" :disabled="config.disabled" :depth="depth" />
    <p v-if="config.helpText" class="ui-property-help" v-localize="config.helpText"></p>
  </ui-property>
</template>


<script>
  import Strings from 'zero/services/strings';
  import Objects from 'zero/services/objects';
  import Localization from 'zero/services/localization';

  export default {
    name: 'uiEditorComponent',

    props: {
      value: {
        type: [ Object, Array ]
      },
      config: {
        type: Object,
        required: true
      },
      renderer: {
        type: Object,
        required: true
      },
      meta: {
        type: Object,
        default: () => { }
      },
      depth: {
        type: Number,
        default: 0
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
      renderer: {
        deep: true,
        handler: function ()
        {
          this.rebuildConfig();
        }
      }
    },

    data: () => ({
      fieldComponent: null,
      model: null,
      selector: [],
      classList: [],
      label: null,
      description: null
    }),

    mounted()
    {
      this.rebuildModel();
    },

    computed: {
      view()
      {
        return this.config.display;
      },
      classes()
      {
        
      },
      fulfillsCondition()
      {
        return typeof this.config.condition !== 'function' || this.config.condition(this.value);
      }
    },

    created()
    {
      this.fieldComponent = () =>
      {
        if (this.view === 'custom')
        {
          if (this.config.path.indexOf('@zero') === 0)
          {
            return import(`zero/` + this.config.path.substring(6));
          }
          else if (this.config.path.indexOf('@shop') === 0) // TODO common system for plugins
          {
            return import(`shop/` + this.config.path.substring(6));
          }
          else
          {
            // TODO external imports
          }
        }
        else if (this.view === 'renderer')
        {
          return import(`zero/editor/editor`);
        }
        else
        {
          return import(`zero/editor/fields/${this.view.toLowerCase()}`);
        }
      }

      this.rebuildModel();
      this.rebuildConfig();
    },

    methods: {

      rebuildModel()
      {
        this.selector = Strings.selectorToArray(this.config.field);
        var currentValue = this.value;

        if (!this.selector || !this.selector.length || !currentValue)
        {
          this.model = null;
        }
        else
        {
          for (var key of this.selector)
          {
            if (key in currentValue)
            {
              currentValue = currentValue[key];
            }
            else
            {
              break;
            }
          }

          this.model = currentValue;
        }
      },


      rebuildConfig()
      {
        // build class list
        let classes = typeof this.config.class === 'string' ? this.config.class.split(' ') : (this.config.class || []);
        if (this.view === 'renderer' || this.view === 'nested')
        {
          classes.push('full-width');
        }
        if (this.config.hideLabel)
        {
          classes.push('hide-label');
        }
        if (this.depth > 0)
        {
          classes.push('is-nested');
        }
        this.classList = classes;


        // build label
        let label = null;
        if (this.config.label && this.config.label.indexOf('@') === 0)
        {
          label = this.config.label;
        }
        else if (this.renderer.labelTemplate)
        {
          label = this.renderer.labelTemplate(this.config.label || this.config.field);
        }
        else
        {
          label = this.config.label || this.config.field;
        }
        this.label = Localization.localize(label);


        // build description
        let description = null;
        if (this.config.description && this.config.description.indexOf('@') === 0)
        {
          description = this.config.description;
        }
        else if (this.config.description === null)
        {
          description = null;
        }
        else if (this.renderer.descriptionTemplate)
        {
          description = this.renderer.descriptionTemplate(this.config.description || this.config.field);
        }
        else
        {
          description = this.config.description;
        }
        this.description = Localization.localize(description, { hideEmpty: true });
      },


      onChange(value)
      {
        Objects.setValue(this.value, this.selector, value);
        this.$emit('input', this.value);
      }
    }
  }
</script>
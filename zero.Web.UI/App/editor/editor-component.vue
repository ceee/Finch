<template>
  <ui-property :field="params.field" :label="params.label" :description="params.description" :required="params.required" :class="classes" :is-text="view === 'output'">
    <component v-if="fieldComponent" :is="fieldComponent" :component="component" :config="params" :value="model" @input="onChange" />
  </ui-property>
</template>


<script>
  import Strings from 'zero/services/strings';
  import Objects from 'zero/services/objects';

  export default {
    name: 'uiEditorComponent',

    props: {
      value: {
        type: [ Object, Array ]
      },
      field: {
        type: String,
        required: true
      },
      component: {
        type: Object,
        required: true
      },
      deep: {
        type: Boolean,
        default: false
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
    },

    data: () => ({
      fieldComponent: null,
      model: null,
      selector: []
    }),

    mounted()
    {
      this.rebuildModel();
    },

    computed: {
      params()
      {
        return this.component.params;
      },
      view()
      {
        return this.component.params.view;
      },
      classes()
      {
        let classes = this.component.params.options.classes;
        if (this.view === 'nested')
        {
          classes.push('full-width');
        }
        if (this.component.params.options.hideLabel)
        {
          classes.push('hide-label');
        }
        return classes;
      }
    },

    created()
    {
      this.fieldComponent = () =>
      {
        console.info(this.view);
        if (this.view === 'custom')
        {
          //return import('@/Plugins/' + this.component.params.componentPath);
        }
        else if (this.view === 'nested')
        {
          return import(`zero/editor/editor-nested`);
        }
        else if (this.view === 'renderer')
        {
          return import(`zero/editor/editor-nested`);
        }
        else
        {
          return import(`zero/editor/fields/${this.view}`);
        }
      }
    },

    methods: {

      rebuildModel()
      {
        this.selector = Strings.selectorToArray(this.field);
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

      onChange(value)
      {
        Objects.setValue(this.value, this.selector, value);
        this.$emit('input', this.value);
      }
    }
  }
</script>
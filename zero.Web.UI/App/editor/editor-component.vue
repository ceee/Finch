<template>
  <ui-property :field="params.field" :label="params.label" :description="params.description" :required="params.required" :class="classes" :is-text="view === 'output'">
    <component v-if="fieldComponent" :is="fieldComponent" :component="component" :config="params" :value="value" @input="onChange" />
  </ui-property>
</template>


<script>
  import Axios from 'axios';

  export default {
    name: 'uiEditorComponent',

    props: {
      value: {
        type: [ Object, Number, Array, String, Boolean, Date ]
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

    data: () => ({
      fieldComponent: null
    }),

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
        return classes;
      }
    },

    created()
    {
      this.fieldComponent = () =>
      {
        if (this.view === 'custom')
        {
          //return import('@/Plugins/' + this.component.params.componentPath);
        }
        else if (this.view === 'nested')
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
      onChange(value)
      {
        this.$emit('input', value);
      }
    }
  }
</script>
<template>
  <ui-property :field="params.field" :label="params.label" :description="params.description" :required="params.required" :class="params.options.classes" :is-text="isText">
    <component v-if="fieldComponent" :is="fieldComponent" :config="params" :value="value" @input="onChange" />
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
      isText()
      {
        return this.component.params.view === 'output';
      }
    },

    created()
    {
      this.fieldComponent = () =>
      {
        if (this.component.params.view === 'custom')
        {
          return import('@/Plugins/' + this.component.params.componentPath);
        }
        else
        {
          return import(`zero/editor/fields/${this.component.params.view}`);
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
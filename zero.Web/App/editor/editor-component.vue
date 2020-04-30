<template>
  <ui-property :field="params.field" :label="params.label" :description="params.description" :required="params.required" :class="params.options.classes">
    <component v-if="fieldComponent" :is="fieldComponent" />
  </ui-property>
</template>


<script>
  import Axios from 'axios';

  export default {
    name: 'uiEditor',

    props: {
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
      }
    },

    data: () => ({
      
    }),

    created()
    {
      this.fieldComponent = () => import(`zero/editor/fields/${this.component.params.view}`);
    }
  }
</script>
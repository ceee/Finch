<template>
  <component :is="node" class="editor-component">
    {{component.params.view}}
  </component>
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
      method: null,
      node: 'div',
    }),

    created()
    {
      //if (this.component.method === 'box')
      //{
      //  this.node = 'ui-tab';
      //}
    }
  }
</script>
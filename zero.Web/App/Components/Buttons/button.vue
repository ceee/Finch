<template>
  <button class="ui-button" :class="typeClass">
    <span v-html="label"></span>
  </button>
</template>


<script>
  export default {
    name: 'uiButton',

    props: {
      label: {
        type: String,
        required: true
      },
      state: {
        type: String,
        default: 'default'
      },
      type: {
        type: String,
        default: 'action'
      },
      disabled: Boolean
    },

    computed: {
      typeClass() 
      {
        return 'type-' + this.type;
      }
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>

<style lang="scss" scoped>
  @import 'Sass/Core/settings';

  
</style>
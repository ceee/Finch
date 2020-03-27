<template>
  <button class="ui-button" :class="{ themeClass }">
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
      theme: {
        type: String,
        default: 'action'
      },
      disabled: Boolean
    },

    computed: {
      themeClass() 
      {
        return 'theme-' + this.theme;
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
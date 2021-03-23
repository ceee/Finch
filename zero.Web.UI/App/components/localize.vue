<template>
  <span>{{output}}</span>
</template>


<script>
  import Localization from 'zero/helpers/localization.js';

  export default {
    name: 'uiLocalize',

    props: {
      value: {
        type: String,
        default: null
      },
      force: {
        type: Boolean,
        default: false
      },
      tokens: {
        type: Object,
        default: null
      },
      hideEmpty: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      output()
      {
        return Localization.localize(this.value, { tokens: this.tokens, force: this.force, hideEmpty: this.hideEmpty });
      }
    }
  }
</script>
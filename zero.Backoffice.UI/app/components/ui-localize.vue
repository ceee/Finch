<template>
  <span>{{output}}</span>
</template>


<script>
  import { localize } from '../services/localization';

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
        return localize(this.value, { tokens: this.tokens, force: this.force, hideEmpty: this.hideEmpty });
      }
    }
  }
</script>
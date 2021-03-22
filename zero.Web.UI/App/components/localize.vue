<template>
  <!-- // TODO can we replace this in Vue 3 with just {{output}}? <span> will destroy layout -->
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
        default: () =>
        {
          return {}
        }
      },
      hideEmpty: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      output: null
    }),

    watch: {
      value: function (value)
      {
        this.rebuild();
      }
    },

    mounted()
    {
      this.rebuild();
    },

    methods: {

      rebuild()
      {
        this.output = Localization.localize(this.value, { tokens: this.tokens, force: this.force, hideEmpty: this.hideEmpty });
      }
    }
  }
</script>
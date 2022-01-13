<template>
  <span class="ui-property-output" v-localize="{ key: output, html: html }"></span>
</template>

<script>
  export default {
    props: {
      value: Object,
      config: Object,

      html: {
        type: Boolean,
        default: false
      },
      render: Function
    },

    computed: {
      output()
      {
        return typeof this.render === 'function' ? this.render(this.value, this.config.model) : this.value;
      }
    }
  }
</script>
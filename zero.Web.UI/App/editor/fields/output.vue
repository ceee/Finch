<template>
  <span class="ui-property-output" v-localize="output"></span>
</template>

<script>
  export default {
    props: {
      value: [Object, String, Array, Number],
      config: Object
    },

    computed: {
      output()
      {
        return typeof this.config.render === 'function' ? this.config.render(this.value, this.config) : this.value;
      }
    }
  }
</script>
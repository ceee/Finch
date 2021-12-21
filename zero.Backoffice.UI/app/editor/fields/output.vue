<template>
  <span class="ui-property-output" v-localize="output"></span>
</template>

<script>
  export default {
    props: {
      value: Object,
      config: Object,

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
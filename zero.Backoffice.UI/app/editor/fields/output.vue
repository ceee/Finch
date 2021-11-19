<template>
  <span class="ui-property-output" v-localize="output"></span>
</template>

<script>
  export default {
    props: {
      value: [Object, String, Array, Number, Boolean],
      entity: Object,
      render: Function
    },

    computed: {
      output()
      {
        return typeof this.render === 'function' ? this.render(this.value, this.entity) : this.value;
      }
    }
  }
</script>
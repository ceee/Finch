<template>
  <ui-rte :value="value" @input="$emit('input', $event)" :disabled="config.disabled" v-bind="{ maxLength, placeholder, setup }" />
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object,

      maxLength: {
        type: Number,
        default: null
      },
      placeholder: {
        type: String,
        default: null
      },
      setup: {
        type: Function,
        default: () => { }
      }
    }
  }
</script>
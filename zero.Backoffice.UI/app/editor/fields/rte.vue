<template>
  <ui-rte :value="value" @input="$emit('input', $event)" :disabled="disabled" v-bind="{ maxLength, placeholder, setup }" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
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
      },
      config: Object
    }
  }
</script>
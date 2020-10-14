<template>
  <ui-rte :value="value" @input="$emit('input', $event)" :disabled="disabled" />
</template>


<script>
  export default {
    emits: ['input'],

    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    }
  }
</script>
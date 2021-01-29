<template>
  <ui-linkpicker :config="config" :value="value" @input="$emit('input', $event)" :disabled="disabled" :limit="limit" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Array],
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      limit: {
        type: Number,
        default: 1
      },
      config: Object
    },
  }
</script>
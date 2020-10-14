<template>
  <ui-mediapicker :config="config" :value="value" @input="$emit('input', $event)" :disabled="disabled" />
</template>


<script>
  export default {
    emits: ['input'],

    props: {
      value: {
        type: [String, Array],
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    },
  }
</script>
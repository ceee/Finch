<template>
  <ui-mediapicker :config="config" :value="value" @input="$emit('input', $event)" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Object, Array],
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
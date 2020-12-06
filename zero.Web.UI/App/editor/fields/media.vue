<template>
  <ui-mediapicker :config="{ limit }" :value="value" @input="$emit('input', $event)" :disabled="disabled" />
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
      }
    }
  }
</script>
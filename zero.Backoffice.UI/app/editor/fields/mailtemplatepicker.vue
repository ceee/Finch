<template>
  <ui-mailtemplatepicker :value="value" @input="$emit('input', $event)" :limit="limit" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: [String, Array],
      disabled: Boolean,
      limit: {
        type: Number,
        default: 1
      }
    }
  }
</script>
<template>
  <ui-toggle :value="value" @input="$emit('input', $event)" :disabled="disabled" :negative="negative" />
</template>


<script>
  export default {
    props: {
      value: {
        type: Boolean,
        default: false
      },
      disabled: {
        type: Boolean,
        default: false
      },
      negative: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
<template>
  <ui-toggle :value="value" @input="$emit('input', $event)" :disabled="disabled" />
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
      config: Object
    }
  }
</script>
<template>
  <ui-toggle :value="value" @input="$emit('input', $event)" :disabled="disabled" :negative="config.negative || false" />
</template>


<script>
  export default {
    emits: ['input'],

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
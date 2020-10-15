<template>
  <ui-input-list :value="value" @input="$emit('input', $event)" :add-label="config.addLabel || '@ui.add'" :disabled="disabled" :inline="true" />
</template>

<script>
  export default {
    emits: ['input'],

    props: {
      value: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    },
  }
</script>
<template>
  <ui-tags :value="value" @input="$emit('input', $event)" :add-label="config.addLabel || '@ui.add'" :disabled="disabled" />
</template>

<script>
  export default {
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
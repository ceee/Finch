<template>
  <ui-input-list :value="value" @input="$emit('input', $event)" :add-label="config.addLabel || '@ui.add'" />
</template>

<script>
  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      },
      config: Object
    },
  }
</script>
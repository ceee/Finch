<template>
  <ui-check-list :value="value" @input="$emit('input', $event)" :reverse="config.reverse" :items="config.items" :disabled="disabled" />
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
<template>
  <ui-tags :value="value" @input="$emit('input', $event)" :max-items="limit" :max-length="maxTagLength" :disabled="config.disabled" />
</template>

<script>
  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      },
      config: Object,
      limit: {
        type: Number,
        default: 10
      },
      maxTagLength: {
        type: Number,
        default: 200
      },
      autocompleteScope: {
        type: String,
        default: null
      }
    },
  }
</script>
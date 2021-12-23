<template>
  <ui-tags :value="value" @input="$emit('input', $event)" :max-items="limit" :max-length="maxItemLength" :disabled="disabled" />
</template>

<script>
  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      },
      disabled: Boolean,
      limit: {
        type: Number,
        default: 10
      },
      maxItemLength: {
        type: Number,
        default: 200
      }
    },
  }
</script>
<template>
  <ui-input-list :value="value" @input="$emit('input', $event)" :add-label="addLabel" :disabled="disabled" 
                 :max-items="limit"
                 :inline="true" :max-length="maxItemLength" />
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
      limit: {
        type: Number,
        default: 10
      },
      maxItemLength: {
        type: Number,
        default: 10
      },
      addLabel: {
        type: String,
        default: '@ui.add'
      }
    },
  }
</script>
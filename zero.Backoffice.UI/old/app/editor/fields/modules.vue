<template>
  <ui-modules :value="value" @input="$emit('input', $event)" :config="config" :tags="tags" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Object, Array]
      },
      disabled: {
        type: Boolean,
        default: false
      },
      tags: {
        type: Array,
        default: () => []
      },
      config: Object
    }
  }
</script>
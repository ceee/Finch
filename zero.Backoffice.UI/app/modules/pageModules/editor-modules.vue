<template>
  <ui-modules :value="value" @input="$emit('input', $event)" />
</template>


<script>
  export default {
    props: {
      value: Array,
      config: Object,
      limit: {
        type: Number,
        default: 1
      }
    }
  }
</script>
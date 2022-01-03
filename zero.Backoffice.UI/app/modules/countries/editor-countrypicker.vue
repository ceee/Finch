<template>
  <ui-countrypicker :value="value" @input="$emit('input', $event)" :limit="limit" :disabled="config.disabled" />
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object,
      limit: {
        type: Number,
        default: 1
      }
    }
  }
</script>
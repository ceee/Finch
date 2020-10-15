<template>
  <ui-countrypicker :value="value" @input="$emit('input', $event)" :limit="config.limit || 1" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Array]
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    }
  }
</script>
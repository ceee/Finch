<template>
  <ui-modules :value="value" @input="$emit('input', $event)" :config="config" :disabled="disabled" />
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
      config: Object
    }
  }
</script>
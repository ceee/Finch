<template>
  <ui-iconpicker :value="value" @input="$emit('input', $event)" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    }
  }
</script>
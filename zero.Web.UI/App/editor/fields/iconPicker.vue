<template>
  <ui-iconpicker :value="value" @input="$emit('input', $event)" :disabled="disabled" :set="set" />
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
      set: {
        type: String,
        default: 'feather'
      },
      config: Object
    }
  }
</script>
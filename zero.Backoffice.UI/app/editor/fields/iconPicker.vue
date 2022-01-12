<template>
  <ui-iconpicker :value="value" @input="$emit('input', $event)" :disabled="config.disabled" :set="set" :colors="colors" />
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
      colors: {
        type: Boolean,
        default: false
      },
      config: Object
    }
  }
</script>
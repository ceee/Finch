<template>
  <ui-check-list :value="value" @input="$emit('input', $event)" 
                 :reverse="reverse" 
                 :items="items" 
                 :limit="limit"
                 :label-key="labelKey"
                 :id-key="idKey"
                 :disabled="config.disabled" />
</template>

<script>
  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      },
      config: Object,
      items: {
        type: [Array, Function, Promise],
        required: true
      },
      limit: {
        type: Number,
        default: 100
      },
      reverse: Boolean,
      labelKey: {
        type: String,
        default: 'value'
      },
      idKey: {
        type: String,
        default: 'key'
      }
    },
  }
</script>
<template>
  <ui-linkpicker :config="config" :value="value" @input="$emit('input', $event)" :disabled="disabled" v-bind="{ disabled, limit, title, label, target, suffix, areas }" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Array],
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      limit: {
        type: Number,
        default: 1
      },
      title: {
        type: Boolean,
        default: true
      },
      label: {
        type: Boolean,
        default: false
      },
      target: {
        type: Boolean,
        default: true
      },
      suffix: {
        type: Boolean,
        default: false
      },
      areas: {
        type: Array,
        default: () => []
      },
      config: Object
    },
  }
</script>
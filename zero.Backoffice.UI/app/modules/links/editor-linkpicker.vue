<template>
  <ui-linkpicker :value="value" @input="$emit('input', $event)" :limit="limit" :disabled="config.disabled" 
                    :areas="areas" :allow-title="allowTitle" :allow-target="allowTarget" :allow-suffix="allowSuffix" />
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object,
      limit: {
        type: Number,
        default: 1
      },
      areas: {
        type: Array,
        default: []
      },
      allowTitle: {
        type: Boolean,
        default: true
      },
      allowTarget: {
        type: Boolean,
        default: true
      },
      allowSuffix: {
        type: Boolean,
        default: true
      }
    }
  }
</script>
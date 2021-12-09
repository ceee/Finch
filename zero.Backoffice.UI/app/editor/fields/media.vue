<template>
  <ui-mediapicker :config="{ limit, disallowSelect, disallowUpload, fileExtensions, maxFileSize, shared: meta.isShared }" :value="value" @input="$emit('input', $event)" :disabled="disabled" />
</template>


<script>
  export default {
    name: 'uiMediaField',

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
      disallowSelect: {
        type: Boolean,
        default: false
      },
      disallowUpload: {
        type: Boolean,
        default: false
      },
      fileExtensions: {
        type: Array,
        default: null
      },
      maxFileSize: {
        type: Number,
        default: 10
      },
      config: Object,
      meta: Object
    }
  }
</script>
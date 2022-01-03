<template>
  <ui-mediapicker :value="value" 
                  @input="$emit('input', $event)" 
                  :limit="limit" 
                  :for="for" 
                  :disable-select="disableSelect" 
                  :disable-upload="disableUpload" 
                  :file-extensions="fileExtensions" 
                  :disabled="config.disabled" />
</template>


<script>
  export default {
    props: {
      value: [String, Array],
      config: Object,
      limit: {
        type: Number,
        default: 1
      },
      for: {
        type: String,
        default: 'all'
      },
      disableSelect: {
        type: Boolean,
        default: false
      },
      disableUpload: {
        type: Boolean,
        default: false
      },
      fileExtensions: {
        type: Array,
        default: []
      },
    }
  }
</script>
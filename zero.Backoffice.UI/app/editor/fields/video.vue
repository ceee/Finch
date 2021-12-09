<template>
  <video-picker :value="value" @input="$emit('input', $event)" :disabled="disabled" v-bind="{ disabled, limit }" title="Select a video" />
</template>


<script>
  import VideoPicker from 'zero/components/pickers/videoPicker/videopicker.vue';

  export default {
    props: {
      value: {
        type: [Object, Array],
        default: null
      },
      limit: {
        type: Number,
        default: 1
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    },

    components: { VideoPicker }
  }
</script>
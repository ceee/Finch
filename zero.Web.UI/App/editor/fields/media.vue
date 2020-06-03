<template>
  <ui-mediapicker :config="config" :value="value" @input="$emit('input', $event)" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Array],
        default: null
      },
      config: Object
    },
  }
</script>
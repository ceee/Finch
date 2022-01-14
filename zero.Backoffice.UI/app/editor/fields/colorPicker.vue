<template>
  <ui-colorpicker :value="value" @input="$emit('input', $event)" :disabled="config.disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String
      },
      config: Object
    }
  }
</script>
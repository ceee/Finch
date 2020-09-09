<template>
  <ui-colorpicker :value="value" @input="$emit('input', $event)" />
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
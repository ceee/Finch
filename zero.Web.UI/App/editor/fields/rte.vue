<template>
  <ui-rte :value="value" @input="$emit('input', $event)" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      config: Object
    }
  }
</script>
<template>
  <ui-toggle :value="value" @input="$emit('input', $event)" />
</template>


<script>
  export default {
    props: {
      value: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
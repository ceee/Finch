<template>
  <ui-toggle :value="value" @input="$emit('input', $event)" :disabled="disabled" :negative="negative" />
</template>


<script>
  export default {
    props: {
      value: Boolean,
      disabled: Boolean,
      negative: Boolean
    }
  }
</script>
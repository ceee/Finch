<template>
  <ui-radio-button :items="items" :value="value" @input="$emit('input', $event)" :disabled="config.disabled" :horizontal="horizontal" />
</template>


<script>
  export default {
    props: {
      value: [String, Number],
      horizontal: {
        type: Boolean,
        default: true
      },
      config: Object,
      items: [Array, Function]
    }
  }
</script>
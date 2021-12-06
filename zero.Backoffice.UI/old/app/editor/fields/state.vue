<template>
  <ui-state-button :items="items" :value="value" @input="$emit('input', $event)" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [ String, Number ]
      },
      disabled: {
        type: Boolean,
        default: false
      },
      items: [Array, Function]
    }
  }
</script>
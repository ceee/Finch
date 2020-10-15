<template>
  <textarea class="ui-textarea" :value="value" @input="$emit('input', $event.target.value)" rows="3" :disabled="disabled"></textarea>
</template>


<script>
  export default {
    props: {
      value: {
        type: String
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    },
  }
</script>
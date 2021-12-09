<template>
  <ui-toggle :model-value="modelValue" @input="$emit('input', $event)" v-bind="{ disabled, negative, onContent, offContent }" />
</template>


<script>
  export default {
    props: {
      modelValue: Boolean,
      disabled: Boolean,
      negative: Boolean,
      onContent: String,
      offContent: String
    }
  }
</script>
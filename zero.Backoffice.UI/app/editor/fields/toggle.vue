<template>
  <ui-toggle :on="value" @update:on="$emit('input', $event)" :disabled="disabled" v-bind="{ negative, onContent, offContent }" />
</template>


<script>
  export default {
    props: {
      value: Boolean,
      entity: Object,
      disabled: Boolean,

      negative: Boolean,
      onContent: String,
      offContent: String
    }
  }
</script>
<template>
  <ui-toggle :on="value" @update:on="$emit('input', $event)" v-bind="{ disabled, negative, onContent, offContent }" />
</template>


<script>
  export default {
    props: {
      value: Boolean,
      disabled: Boolean,
      negative: Boolean,
      onContent: String,
      offContent: String
    }
  }
</script>
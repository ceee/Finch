<template>
  <ui-toggle :on="value" @update:on="$emit('input', $event)" :disabled="config.disabled" v-bind="{ negative, onContent, offContent }" :content-left="true" />
</template>


<script>
  export default {
    props: {
      value: Boolean,
      config: Object,

      negative: Boolean,
      onContent: String,
      offContent: String
    }
  }
</script>
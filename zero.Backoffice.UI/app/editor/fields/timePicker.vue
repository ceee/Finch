<template>
  <div class="ui-box">
    <ui-timepicker :value="value" @input="$emit('input', $event)" :disabled="config.disabled"
      v-bind="{ format, amPm }" />
  </div>
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object,
      format: {
        type: String,
        default: null
      },
      amPm: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
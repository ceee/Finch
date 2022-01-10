<template>
  <div class="ui-box">
    <ui-datepicker :value="value" @input="$emit('input', $event)" :disabled="config.disabled"
      v-bind="{ format, time: pickTime, maxDate, minDate, amPm }" />
  </div>
</template>


<script>
  export default {
    props: {
      value: [String, Date],
      config: Object,
      format: {
        type: String,
        default: null
      },
      pickTime: {
        type: Boolean,
        default: false,
      },
      maxDate: {
        type: [String, Date],
        default: null
      },
      minDate: {
        type: [String, Date],
        default: null
      },
      amPm: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
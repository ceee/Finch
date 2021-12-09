<template>
  <div class="ui-box">
    <ui-daterangepicker :value="value" @input="$emit('input', $event)" :disabled="disabled"
      v-bind="{ format, time, maxDate, minDate, fromLabel, toLabel, amPm, inline }" />
  </div>
</template>


<script>
  export default {
    props: {
      value: {
        type: Object,
        default: {
          from: null,
          to: null
        }
      },
      format: {
        type: String,
        default: null
      },
      time: {
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
      fromLabel: {
        type: String,
        default: '@ui.date.range_from'
      },
      toLabel: {
        type: String,
        default: '@ui.date.range_to' 
      },
      amPm: {
        type: Boolean,
        default: false
      },
      inline: {
        type: Boolean,
        default: false
      },
      disabled: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
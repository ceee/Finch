<template>
  <div class="ui-box">
    <ui-daterangepicker :value="value" @input="$emit('input', $event)" :disabled="config.disabled"
                        v-bind="{ format, time: pickTime, maxDate, minDate, fromLabel, toLabel, amPm, inline }" />
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
      }
    }
  }
</script>
<template>
  <div class="ui-box">
    <ui-datepicker :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled"
      v-bind="{ format, time, maxDate, minDate, amPm }" />
  </div>
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Date],
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
      amPm: {
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
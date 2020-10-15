<template>
  <div class="ui-box">
    <ui-daterangepicker :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled"
      :format="config.format"
      :time="config.time || false"
      :max ="config.max" :min="config.min"
      :from-text="config.fromText || '@ui.date.range_from'"
      :to-text="config.toText || '@ui.date.range_to'"
      :amPm="config.amPm || false"
      :inline="config.inline || false" />
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
      disabled: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
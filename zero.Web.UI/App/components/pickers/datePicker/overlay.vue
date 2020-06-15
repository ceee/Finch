<template>
  <div ref="calendar" class="ui-datepicker-overlay"></div>
</template>


<script>
  import flatpickr from 'flatpickr';
  import { extend as _extend } from 'underscore';

  export default {

    props: {
      value: String,
      options: {
        type: Object,
        default: () => { }
      }
    },

    data: () => ({
      date: null,
      initialized: false
    }),

    mounted()
    {
      let vm = this;

      if (this.initialized)
      {
        return;
      }

      flatpickr(this.$refs.calendar, _extend({
        inline: true,
        enableTime: true,
        time_24hr: true,
        defaultDate: this.value,
        minuteIncrement: 1,
        onChange(dates)
        {
          vm.$emit('change', dates[0]);
        },
      }, this.options));

      this.initialized = true;
    }
  }
</script>

<style lang="scss">
  .ui-datepicker-overlay
  {
    
  }
</style>
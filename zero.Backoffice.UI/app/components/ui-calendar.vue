<template>
  <div ref="calendar" class="ui-calendar"></div>
</template>


<script>
  import flatpickr from 'flatpickr';
  import { extendObject } from '../utils/objects';

  export default {
    name: 'uiCalendar',

    props: {
      today: String,
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

      flatpickr(this.$refs.calendar, extendObject({
        inline: true,
        enableTime: true,
        time_24hr: true,
        defaultDate: this.today,
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
  .ui-calendar
  {
    
  }
</style>
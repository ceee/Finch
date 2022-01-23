<template>
  <div ref="calendar" class="ui-calendar"></div>
</template>


<script>
  import flatpickr from 'flatpickr';
  import { German } from "flatpickr/dist/l10n/de.js"
  import { extendObject } from '../utils/objects';
  import { useAccountStore } from '../account/store';

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

      const user = useAccountStore();

      flatpickr(this.$refs.calendar, extendObject({
        inline: true,
        enableTime: true,
        time_24hr: true,
        defaultDate: this.today,
        minuteIncrement: 1,
        locale: user.user && user.user.culture == 'de-DE' ? German : null,
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
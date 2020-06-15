<template>
  <div class="ui-calendar">

  </div>
</template>


<script>
  import dayjs from 'dayjs';
  import Strings from 'zero/services/strings';

  export default {
    name: 'uiCalendar',

    props: {
      value: {
        type: [String, Date],
        default: null
      }
    },

    data: () => ({
      selected: null,
      current: null
    }),

    watch: {
      value: function (value)
      {
        this.rebuild();
      }
    },

    mounted()
    {
      this.rebuild();
    },

    methods: {

      rebuild()
      {
        if (!this.value)
        {
          this.selected = dayjs();
        }
        else
        {
          this.selected = dayjs(this.value);
        }

        console.info(this.selected);
      }

    }

  }
</script>
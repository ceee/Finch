<template>
  <span class="ui-date" v-html="output"></span>
</template>


<script>
  import dayjs from 'dayjs';

  const DATETIME_FORMAT = 'DD.MM.YY HH:mm';
  const DATE_FORMAT = 'DD.MM.YY';

  export default {
    name: 'uiDate',

    props: {
      value: {
        type: String,
        default: null
      }
    },

    data: () => ({
      output: null
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
          this.output = '';
          return;
        }

        this.output = dayjs(this.value).format(DATE_FORMAT);
      }

    }

  }
</script>
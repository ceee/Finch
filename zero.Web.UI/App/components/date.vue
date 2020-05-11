<template>
  <span class="ui-date" v-html="output"></span>
</template>


<script>
  import dayjs from 'dayjs';
  import Strings from 'zero/services/strings';

  export default {
    name: 'uiDate',

    props: {
      value: {
        type: String,
        default: null
      },
      format: {
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

        this.output = Strings.date(this.value, this.format);
      }

    }

  }
</script>
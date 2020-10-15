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
      },
      split: {
        type: Boolean,
        default: false
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

        if (!this.split)
        {
          this.output = Strings.date(this.value, this.format);
        }
        else
        {
          this.output = Strings.date(this.value, 'short') + ' <span class="-minor">' + Strings.date(this.value, 'time') + '</span>';
        }
      }

    }

  }
</script>

<style lang="scss">
  .ui-date .-minor
  {
    color: var(--color-text-dim);
  }
</style>
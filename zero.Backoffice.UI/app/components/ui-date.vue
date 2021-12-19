<template>
  <span class="ui-date" v-html="output"></span>
</template>


<script>
  import { formatDate } from '../utils/dates';

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
          this.output = formatDate(this.value, this.format);
        }
        else
        {
          this.output = formatDate(this.value, 'short') + ' <span class="-minor">' + formatDate(this.value, 'time') + '</span>';
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
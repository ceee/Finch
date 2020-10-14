<template>
  <section v-show="active" class="ui-tab" :aria-hidden="!active" role="tabpanel">
    <slot />
  </section>
</template>


<script>
  import Strings from '@zero/services/strings.js';

  export default {
    name: 'uiTab',

    props: {
      label: {
        type: String,
        required: true
      },
      count: {
        type: Number,
        default: 0
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      id: null,
      loaded: false,
      active: false,
      hasErrors: false,
      countOutput: 0
    }),

    watch: {
      active(val)
      {
        this.loaded = true;
      },
      count(val)
      {
        this.countOutput = val;
      }
    },

    created()
    {
      this.id = Strings.guid();
      this.loaded = this.active;
      this.countOutput = this.count;
    },

    methods: {

      setCount(count)
      {
        this.countOutput = count;
      },

      // set and display errors
      setErrors(errors, append)
      {
        this.hasErrors = !!errors;
      },

      // clear errors and hide
      clearErrors()
      {
        this.hasErrors = false;
      }

    }
  }
</script>
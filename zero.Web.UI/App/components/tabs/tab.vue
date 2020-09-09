<template>
  <section v-show="active" class="ui-tab" :aria-hidden="!active" role="tabpanel">
    <slot />
  </section>
</template>


<script>
  import Strings from 'zero/services/strings';

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
      hasErrors: false
    }),

    watch: {
      active(val)
      {
        this.loaded = true;
      }
    },

    mounted ()
    {
      this.id = Strings.guid();
      this.loaded = this.active;
    },

    methods: {

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
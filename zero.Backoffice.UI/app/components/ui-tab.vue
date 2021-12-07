<template>
  <section v-show="active" class="ui-tab" :aria-hidden="!active" role="tabpanel">
    <slot />
  </section>
</template>


<script>
  import { generateId } from '../utils';

  export default {
    name: 'uiTab',

    props: {
      label: {
        type: String
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
      this.id = generateId();
      this.loaded = this.active;
      this.countOutput = this.count;
      this.$parent.register(this);
    },

    unmounted()
    {
      this.$parent.unregister(this);
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
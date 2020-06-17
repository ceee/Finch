<template>
  <section v-if="loaded" v-show="active" class="ui-tab" :aria-hidden="!active" role="tabpanel">
    <slot />
  </section>
</template>


<script>
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
      loaded: false,
      active: false
    }),

    watch: {
      active(val)
      {
        this.loaded = true;
      }
    },

    mounted ()
    {
      this.loaded = this.active;
    },

    methods: {

    }
  }
</script>
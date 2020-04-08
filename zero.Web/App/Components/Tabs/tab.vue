<template>
  <section v-show="active" class="ui-tab" :aria-hidden="!active" role="tabpanel">
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
      active: false
    }),

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>
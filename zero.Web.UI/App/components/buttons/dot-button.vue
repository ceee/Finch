<template>
  <button type="button" class="ui-dot-button" :disabled="disabled" @click="tryClick" v-localize:title="title">
    <span class="sr-only" v-localize="title"></span>
    <i class="ui-button-icon fth-more-horizontal"></i>
  </button>
</template>


<script>
  export default {
    name: 'uiDotButton',

    emits: ['click'],

    props: {
      state: {
        type: String,
        default: 'default'
      },
      title: {
        type: String,
        default: '@ui.actions'
      },
      disabled: Boolean
    },

    mounted ()
    {
      
    },

    methods: {

      tryClick(ev)
      {
        this.$emit('click', ev);
      }
    }
  }
</script>
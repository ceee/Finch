<template>
  <button type="button" class="ui-dot-button" :disabled="disabled" @click="tryClick" :title="title | localize">
    <span class="sr-only" v-localize="title"></span>
    <ui-icon class="ui-button-icon" symbol="fth-more-horizontal" />
  </button>
</template>


<script>
  export default {
    name: 'uiDotButton',

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
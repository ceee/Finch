<template>
  <button type="button" class="ui-dot-button" @click="click" :title="title | localize">
    <span class="sr-only" v-localize="title"></span>
    <i class="fth-more-horizontal"></i>
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
      disabled: Boolean,
      click: {
        type: Function,
        default: () => { }
      }
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>
<template>
  <button class="ui-icon-button" @click="click" :title="title | localize">
    <span class="sr-only" v-localize="title"></span>
    <i :class="icon"></i>
  </button>
</template>


<script>
  export default {
    name: 'uiIconButton',

    props: {
      state: {
        type: String,
        default: 'default'
      },
      icon: {
        type: String,
        default: 'fth-arrow-left'
      },
      title: {
        type: String,
        default: '@ui.back'
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
<template>
  <button type="button" class="ui-icon-button" :disabled="disabled" :class="'type-' + type.split(' ').join(' type-')" @click="tryClick" v-localize:title="title">
    <span class="sr-only" v-localize="title"></span>
    <i class="ui-button-icon" :class="icon"></i>
  </button>
</template>


<script>
  export default {
    name: 'uiIconButton',

    emits: ['click'],

    props: {
      state: {
        type: String,
        default: 'default'
      },
      icon: {
        type: String,
        default: 'fth-arrow-left'
      },
      type: {
        type: String,
        default: 'action'
      },
      title: {
        type: String,
        default: ''
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
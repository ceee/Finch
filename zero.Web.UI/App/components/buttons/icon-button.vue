<template>
  <button type="button" class="ui-icon-button" :disabled="disabled" :class="'type-' + type.split(' ').join(' type-')" @click="tryClick" :title="title | localize">
    <span class="sr-only" v-localize="title"></span>
    <ui-icon class="ui-button-icon" :symbol="icon" :size="size" :stroke="stroke" />
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
      stroke: {
        type: Number,
        default: 2
      },
      type: {
        type: String,
        default: 'action'
      },
      title: {
        type: String,
        default: ''
      },
      size: {
        type: Number,
        default: 16
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
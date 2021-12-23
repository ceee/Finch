<template>
  <button type="button" class="ui-icon-button" :disabled="disabled" :class="'type-' + type.split(' ').join(' type-')" @click="$emit('click', $event)" v-localize:title="title">
    <span class="sr-only" v-localize="title"></span>
    <ui-icon class="ui-button-icon" :symbol="icon" :size="size" :stroke="stroke" />
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
        default: 14
      },
      disabled: Boolean
    }
  }
</script>
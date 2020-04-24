<template>
  <button type="button" class="ui-select-button type-light" :disabled="disabled" @click="tryClick">
    <i class="ui-select-button-icon" :class="icon"></i>
    <content class="ui-select-button-content">
      <strong class="ui-select-button-label" v-localize="label"></strong>
      <span class="ui-select-button-description" v-if="description" v-localize:html="description"></span>
    </content>
  </button>
</template>


<script>
  export default {
    name: 'uiIconButton',

    props: {
      icon: {
        type: String,
        default: 'fth-box'
      },
      label: {
        type: String
      },
      description: {
        type: String,
        default: null
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
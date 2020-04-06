<template>
  <button class="ui-button" :class="buttonClass" @click="click">
    <span v-html="label"></span>
    <i v-if="caret" :class="caretClass"></i>
    <i v-if="icon" :class="icon"></i>
  </button>
</template>


<script>
  export default {
    name: 'uiButton',

    props: {
      label: {
        type: String,
        required: true
      },
      state: {
        type: String,
        default: 'default'
      },
      type: {
        type: String,
        default: 'action'
      },
      icon: {
        type: String
      },
      caret: String,
      caretPosition: {
        type: String,
        default: 'right'
      },
      disabled: Boolean,
      click: {
        type: Function,
        default: () => { }
      }
    },

    computed: {
      buttonClass() { return 'type-' + this.type.split(' ').join(' type-') + ' caret-' + this.caretPosition + (!!this.icon ? ' has-icon' : ''); },
      caretClass() { return 'fth-chevron-' + this.caret; }
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>
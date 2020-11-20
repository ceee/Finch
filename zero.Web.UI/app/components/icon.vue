<template>
  <i class="ui-icon" :class="iconClass"></i>
</template>


<script>
  export default {
    name: 'uiIcon',

    props: {
      icon: {
        type: String,
        default: null,
        required: true
      }
    },

    computed: {
      iconClass()
      {
        return this.icon.indexOf('fth-') === 0 ? this.icon : ('fth-' + this.icon);
      }
    }
  }
</script>
<template>
  <div class="app-confirm">
    <h2 class="ui-headline">{{overlay.title}}</h2>
    <p>{{overlay.text}}</p>
    <ui-button type="light" :label="overlay.closeLabel" :click="overlay.close"></ui-button>
    <ui-button :label="overlay.confirmLabel" :click="confirm"></ui-button>
  </div>
</template>


<script>
  import Overlay from 'zeroservices/overlay.js'

  export default {

    props: {
      model: Object,
      overlay: Object
    },

    data: () => ({
      
    }),

    mounted()
    {
      
    },

    methods: {

      confirm()
      {
        this.overlay.confirm({ success: true });
      }

    }
  }
</script>

<style lang="scss">
  .app-confirm
  {
    width: 400px;
  }
</style>
<template>
  <div class="app-confirm">
    <h2 class="ui-headline">{{model.title}}</h2>
    <p>{{model.text}}</p>
    <ui-button label="confirm" :click="confirm"></ui-button>
    <ui-button label="close" :click="overlay.close"></ui-button>
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
    height: 200px;
  }
</style>
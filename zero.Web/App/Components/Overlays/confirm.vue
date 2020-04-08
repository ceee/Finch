<template>
  <div class="app-confirm">
    confirm
  </div>
</template>


<script>
  export default {

    props: [ 'content', 'component' ],

    data: () => ({
      
    }),

    mounted()
    {
      console.info(this.key, this.content, this.component);
    },

    methods: {

    }
  }
</script>

<style lang="scss">
  .app-confirm
  {

  }
</style>
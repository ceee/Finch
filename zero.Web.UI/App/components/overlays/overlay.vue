<template>
  <transition name="overlay">
    overlay #{{key}}
  </transition>
</template>


<script>
  //import Spinner from 'cmp/Generic/spinner.vue'

  export default {

    props: [ 'content', 'component', 'key' ],

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
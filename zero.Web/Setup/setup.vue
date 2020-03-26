<template>
  <div class="app">
  </div>
</template>


<script>
  import Vue from 'vue'
  import Sass from '../Sass/setup.scss'

  export default {
    name: 'setup',

    data: () => ({
      count: 0
    }),

    mounted ()
    {
      
    },

    methods: {

      add()
      {
        this.count += 1;
      }
    }
  }

</script>
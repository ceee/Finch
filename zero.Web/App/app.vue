<template>
  <div class="app">
    <app-navigation />
  </div>
</template>


<script>
  import Vue from 'vue'
  import Sass from '../Sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'

  export default {
    name: 'app',

    components: { AppNavigation },

    data: () => ({}),
  }

</script>
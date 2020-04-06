<template>
  <div class="app">
    <app-navigation />
    <div :is="pageComponent"></div>
  </div>
</template>


<script>
  import Vue from 'vue'
  import Sass from '../Sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'
  import Router from '../router.js'

  export default {
    name: 'app',

    router: Router,

    components: { AppNavigation },

    data: () => ({
      
    }),

    computed: {
      pageComponent()
      {
        return 'app-page';
      },
      path()
      {
        return this.$route.path;
      }
    },

    methods: {
    }
  }

</script>
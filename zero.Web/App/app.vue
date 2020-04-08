<template>
  <div class="app">
    <app-navigation />
    <div class="app-main">
      <router-view></router-view>
    </div>
  </div>
</template>


<script>
  import '../Sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'
  import Router from '../router.js'

  export default {
    name: 'app',

    router: Router,

    components: { AppNavigation },

    data: () => ({
      pageComponent: null
    }),

    mounted()
    {
      
    },

    methods: {
      update()
      {
        
      }
    }
  }

</script>
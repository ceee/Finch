<template>
  <div class="app">
    <app-navigation />
    <div class="app-main">
      <div :is="pageComponent"></div>
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

    watch: {
      '$route': 'update'
    },

    mounted()
    {
      this.update();
    },

    methods: {
      update()
      {
        this.pageComponent = this.$route.path.indexOf('/settings') === 0 ? 'app-settings' : 'app-page';
      }
    }
  }

</script>
<template>
  <div class="app">
    <app-overlays />
    <app-navigation />
    <div class="app-main">
      <router-view></router-view>
    </div>
  </div>
</template>


<script>
  import '../Sass/app.scss'
  import AppNavigation from 'zero/navigation.vue'
  import AppOverlays from 'zerocomponents/Overlays/overlay-holder.vue'
  import Router from '../router.js'
  import 'zero/axios.config.js'

  export default {
    name: 'app',

    router: Router,

    components: { AppNavigation, AppOverlays }
  }

</script>
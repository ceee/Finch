<template>
  <div class="app-confirm">
    <h2 class="ui-headline" v-localize="config.title"></h2>
    <p v-localize:html="config.text"></p>
    <div class="app-confirm-buttons">
      <ui-button type="light" :label="config.closeLabel" @click="config.close"></ui-button>
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/helpers/overlay.js'

  export default {

    props: {
      model: Object,
      config: Object
    }
  }
</script>

<style lang="scss">
  .app-confirm-buttons
  {
    margin-top: 30px;
  }

  .app-confirm p
  {
    line-height: 1.4;
  }
</style>
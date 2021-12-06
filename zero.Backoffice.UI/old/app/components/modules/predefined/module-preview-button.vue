<template>
  <div class="ui-module-preview-button">
    <div class="ui-button type-light" v-text="text"></div>
  </div>
</template>

<script>
  export default {
    name: 'uiModulePreviewButton',

    props: {
      text: {
        type: String
      }
    }
  }
</script>
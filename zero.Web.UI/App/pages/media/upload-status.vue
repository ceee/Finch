<template>
  <div class="ui-media-upload">
    <h2 class="ui-headline" v-localize="'@media.name'"></h2>

    <div class="ui-media-upload-preview">
      
    </div>

    <div class="app-confirm-buttons">
      <!--<ui-button v-if="!disabled" type="action" :submit="true" label="@ui.save"></ui-button>-->
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
    </div>
  </div>
</template>


<script>
  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      loading: false
    }),

    methods: {

      onSubmit()
      {
         
      }
    }
  }
</script>

<style lang="scss">
  .ui-media-upload
  {
    text-align: left;
  }

  .ui-media-upload
  {
    /*height: 200px;
    background: var(--color-bg-mid);
    border-radius: var(--radius) var(--radius) 0 0;
    margin-top: var(--padding);
    padding: 10px;
    overflow: hidden;*/
  }
</style>
<template>
  <ui-overlay-editor class="ui-module-overlay">
    <template v-slot:header>
      <ui-header-bar title="Richtext" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="white" label="@ui.close" @click="config.hide"></ui-button>
      <ui-button label="Confirm" @click="onSave" :state="state"></ui-button>
    </template>

    <div class="ui-module-overlay-editor">
      <ui-editor :config="config.renderer" v-model="model" :meta="meta" :is-page="false" infos="none" />
    </div>
  </ui-overlay-editor>
</template>


<script>
  import UiEditor from 'zero/editor/editor';

  export default {

    props: {
      model: Object,
      config: Object
    },

    components: { UiEditor },

    data: () => ({
      state: 'default',
      meta: {

      }
    }),


    methods: {

      onSave()
      {
        //this.state = 'loading';
        //this.config.confirm(res.model);
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-overlay
  {
    > content
    {
      padding-top: 0 !important;
    }

    .ui-box
    {
      margin: 0;
    }

    .ui-tabs-list
    {
      padding: 0;
      padding-bottom: 32px;
    }

    .ui-property.ui-modules
    {
      margin: 0;
      padding: 0;
    }

    .editor-outer.-infos-aside:not(.is-page)
    {
      display: block;
    }
  }
</style>
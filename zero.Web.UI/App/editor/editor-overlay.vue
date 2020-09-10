<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-overlay-editor class="ui-editor-overlay">

      <template v-slot:header>
        <ui-header-bar :title="config.title" :back-button="false" :close-button="true" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.hide"></ui-button>
        <ui-button type="primary" :submit="true" label="Confirm" :state="form.state" :disabled="loading || disabled"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <div v-if="!loading" class="ui-editor-overlay-editor">
        <ui-editor :config="config.renderer" v-model="model" :meta="meta" :is-page="false" infos="none" />
      </div>

    </ui-overlay-editor>
  </ui-form>
</template>


<script>
  import UiEditor from 'zero/editor/editor';

  export default {

    props: {
      config: Object
    },

    components: { UiEditor },

    data: () => ({
      isAdd: true,
      disabled: false,
      id: null,
      loading: true,
      state: 'default',
      meta: {},
      model: {}
    }),


    methods: {

      onLoad(form)
      {
        this.isAdd = this.config.create === true;
        this.model = JSON.parse(JSON.stringify(this.config.model));
        this.loading = false;
      },


      onSubmit(form)
      {
        this.config.confirm(this.model);
      }
    }
  }
</script>

<style lang="scss">
  .ui-editor-overlay
  {
    > content
    {
      position: relative;
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

    .ui-loading
    {
      position: absolute;
      left: 50%;
      top: 50%;
      margin: -14px 0 0 -14px;
    }
  }
</style>
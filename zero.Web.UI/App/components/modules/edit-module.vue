<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-overlay-editor class="ui-module-overlay">

      <template v-slot:header>
        <ui-header-bar :title="config.module.name" :back-button="false" :close-button="true" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.hide"></ui-button>
        <ui-button v-if="!disabled" type="primary" :submit="true" label="Confirm" :state="form.state" :disabled="loading"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <div v-if="!loading" class="ui-module-overlay-editor">
        <ui-editor :config="editor" v-model="model" :meta="meta" :is-page="false" infos="none" :disabled="disabled" />
      </div>

    </ui-overlay-editor>
  </ui-form>
</template>


<script>
  import ModulesApi from 'zero/resources/modules.js';
  import UiEditor from 'zero/editor/editor.vue';

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
      editor: null,
      model: {}
    }),

    created()
    {
      this.editor = this.config.editor;
    },

    methods: {

      onLoad(form)
      {
        if (this.config.model)
        {
          this.isAdd = false;
          this.model = JSON.parse(JSON.stringify(this.config.model));
        }

        form.load(ModulesApi.getEmpty(this.config.module.alias)).then(response =>
        {
          this.disabled = !response.meta.canEdit || this.config.disabled;
          this.meta = response.meta;

          if (this.isAdd)
          {
            this.model = response.entity;
          }
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        this.config.confirm(this.model);
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-overlay
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
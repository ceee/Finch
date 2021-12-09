<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-overlay-editor class="ui-editor-overlay">

      <template v-slot:header>
        <ui-header-bar :title="config.title" :back-button="false" :close-button="true" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.hide"></ui-button>
        <ui-button v-if="!disabled" type="primary" :submit="true" :label="config.confirmButton || '@ui.confirm'" :state="form.state" :disabled="loading"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <div v-if="!loading" class="ui-editor-overlay-editor">
        <ui-editor :config="editor" v-model="model" :meta="meta" :is-page="false" infos="none" :disabled="disabled" />
      </div>

    </ui-overlay-editor>
  </ui-form>
</template>


<script>
  import UiEditor from './editor.vue';

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
      parent: null,
      editor: null,
      meta: {},
      model: {}
    }),


    methods: {

      onLoad(form)
      {
        this.isAdd = this.config.create === true;
        this.meta = {
          parentModel: this.config.parentModel
        };
        this.model = JSON.parse(JSON.stringify(this.config.model));
        this.editor = this.config.editor;
        this.loading = false;
      },


      onSubmit(form)
      {
        if (typeof this.config.confirmGuard === 'function')
        {
          form.setState('loading');
          this.config.confirmGuard(this.model).then(res =>
          {
            form.setState('success');
            form.setDirty(false);
            this.config.confirm(this.model, res);
          });
        }
        else
        {
          this.config.confirm(this.model);
        }
      }
    }
  }
</script>
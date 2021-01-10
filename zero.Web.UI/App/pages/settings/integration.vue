<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-overlay-editor class="ui-editor-overlay integration">

      <template v-slot:header>
        <ui-header-bar :title="config.model.name" :back-button="false" :close-button="true" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.hide"></ui-button>
        <ui-button v-if="!config.isCreate" type="light onbg" label="@ui.remove" @click="onDelete"></ui-button>
        <ui-button v-if="!disabled" type="primary" :submit="true" label="@ui.save" :state="form.state" :disabled="loading"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <div v-if="!loading" class="ui-editor-overlay-editor">
        <ui-editor :config="editor" v-model="model" :meta="meta" :is-page="false" infos="none" :disabled="disabled" />
      </div>

    </ui-overlay-editor>
  </ui-form>
</template>


<script>
  import UiEditor from 'zero/editor/editor.vue';
  import Overlay from 'zero/helpers/overlay.js';
  import Notification from 'zero/helpers/notification.js';
  import IntegrationsApi from 'zero/api/integrations.js';

  export default {

    props: {
      config: Object
    },

    components: { UiEditor },

    data: () => ({
      disabled: false,
      loading: true,
      state: 'default',
      editor: null,
      meta: {},
      model: {}
    }),

    mounted()
    {
      //this.$el.style.setProperty('--color-primary', this.config.model.color);
      //this.$el.style.setProperty('--color-button', this.config.model.color);
    },

    methods: {

      onLoad(form)
      {
        form.load(this.config.isCreate ? IntegrationsApi.getEmptySettings(this.config.alias) : IntegrationsApi.getSettingsByAlias(this.config.alias)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
          this.editor = this.model.integrationAlias ? 'integration.' + this.model.integrationAlias : null;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        form.handle(IntegrationsApi.save(this.model)).then(res =>
        {
          this.config.confirm(res);
        });
      },


      onDelete()
      {
        Overlay.confirmDelete().then(opts =>
        {
          opts.state('loading');
          IntegrationsApi.delete(this.model.id).then(response =>
          {
            if (response.success)
            {
              opts.state('success');
              opts.hide();
              Notification.success('@deleteoverlay.success', '@deleteoverlay.success_text');
              this.config.close();
            }
            else
            {
              opts.errors(response.errors);
            }
          });
        }); 
      }
    }
  }
</script>
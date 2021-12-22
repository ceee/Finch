<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-trinity class="ui-editor-overlay integration">

      <template v-slot:header>
        <ui-header-bar :title="integration.name" prefix="@integration.list" :back-button="false" :close-button="true" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.close"></ui-button>
        <template v-if="integration.isConfigured">
          <ui-button type="light onbg" label="@ui.remove" @click="onDelete"></ui-button>
          <ui-button v-if="!disabled" type="primary" :submit="true" label="@ui.save" :state="form.state" :disabled="loading"></ui-button>
        </template>
        <template v-if="!integration.isConfigured && !disabled">
          <ui-button type="light onbg" :submit="true" label="@ui.save" :state="form.state" :disabled="loading"></ui-button>
          <ui-button type="accent" @click="saveAndActivate" label="Save and activate" :state="form.state" :disabled="loading"></ui-button>
        </template>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <div v-if="!loading" class="ui-editor-overlay-editor">
        <ui-editor :config="editor" v-model="model" :meta="meta" :is-page="false" infos="none" :disabled="disabled" />
      </div>

    </ui-trinity>
  </ui-form>
</template>


<script>
  import * as overlays from '../../services/overlay';
  //import Notification from 'zero/helpers/notification.js';
  import api from './api';

  export default {

    props: {
      config: Object
    },

    data: () => ({
      integration: {},
      disabled: false,
      loading: true,
      state: 'default',
      editor: null,
      meta: {},
      model: {}
    }),

    created()
    {
      this.integration = { ...this.config.model };
      //this.$el.style.setProperty('--color-primary', this.config.model.color);
      //this.$el.style.setProperty('--color-button', this.config.model.color);
    },

    methods: {

      async onLoad(form)
      {
        const alias = this.integration.alias;
        this.editor = this.integration.editorAlias || 'integration.' + alias;

        const response = await form.load(() => this.integration.isConfigured ? api.getByAlias(alias) : api.getEmpty(alias));
        this.model = response;
        this.loading = false;
      },


      saveAndActivate(e)
      {
        this.model.isActive = true;
        this.onSubmit(this.$refs.form);
      },


      async onSubmit(form)
      {
        const response = this.integration.isConfigured ? await api.update(this.model) : await api.create(this.model);
        await form.handle(response);

        if (response.success)
        {
          this.config.confirm(response.data);
        }
      },


      async onDelete()
      {
        const result = await overlays.confirmDelete();

        if (result.eventType === 'confirm')
        {
          result.state('loading');

          const response = await api.delete(this.integration.alias);

          if (response.success)
          {
            result.state('success');
            result.close();
            //Notification.success('@deleteoverlay.success', '@deleteoverlay.success_text');
            this.config.confirm(response);
          }
        }
      }
    }
  }
</script>
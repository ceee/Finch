<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@integration.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" :title-disabled="true" @delete="onDelete" />
    <ui-editor v-if="editor" :config="editor" v-model="model" :meta="meta" :disabled="disabled">
      <template v-slot:settings-properties>
        <p v-if="!model.isActive" class="ui-message type-error block user-aside-error">
          <i class="ui-message-icon fth-alert-circle"></i>
          <span class="ui-message-text" v-localize="'@integration.activeWarning'"></span>
        </p>
      </template>
    </ui-editor>
  </ui-form>
</template>


<script>
  import IntegrationsApi from 'zero/api/integrations.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    props: ['id', 'alias'],

    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { name: null, integrationAlias: null },
      route: zero.alias.settings.integrations + '-edit',
      disabled: false
    }),

    computed: {
      editor()
      {
        return this.model.integrationAlias ? 'integration.' + this.model.integrationAlias : null;
      }
    },

    methods: {

      onLoad(form)
      {
        form.load(!this.id ? IntegrationsApi.getEmptySettings(this.alias) : IntegrationsApi.getSettingsById(this.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(IntegrationsApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(IntegrationsApi.delete.bind(this, this.id));
      }     
    }
  }
</script>


<style lang="scss">
  .country .country-flag-input
  {
    max-width: 80px;
  }
</style>
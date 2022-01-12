<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-trinity class="ui-editor-overlay">

      <template v-slot:header>
        <ui-header-bar :title="model.title" :back-button="false" :close-button="true" @close="config.close(true)" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.cancel" @click="config.close"></ui-button>
        <ui-button v-if="!disabled" type="accent" :submit="true" :label="model.confirmButton || '@ui.confirm'" :state="form.state" :disabled="loading"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <div v-if="!loading" class="ui-editor-overlay-editor">
        <ui-editor :config="model.editor" v-model="entity" :meta="meta" :is-page="false" infos="none" :disabled="disabled" :scope="true" />
      </div>

    </ui-trinity>
  </ui-form>
</template>


<script>
  export default {
    name: 'uiEditorOverlay',

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      disabled: false,
      id: null,
      loading: true,
      state: 'default',
      parent: null,
      meta: {},
      entity: {}
    }),


    methods: {

      onLoad(form)
      {
        this.meta = {
          //parentModel: this.config.parentModel
        };
        this.entity = JSON.parse(JSON.stringify(this.model.value));
        setTimeout(() =>
        {
          this.loading = false;
        }, 500);
      },


      onSubmit(form)
      {
        if (typeof this.model.confirmGuard === 'function')
        {
          form.setState('loading');
          this.model.confirmGuard(this.entity).then(res =>
          {
            form.setState('success');
            form.setDirty(false);
            this.config.confirm(this.entity, res);
          });
        }
        else
        {
          this.config.confirm(this.entity);
        }
      }
    }
  }
</script>
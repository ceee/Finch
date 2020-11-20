<template>
  <ui-form ref="form" class="mails" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@mailTemplate.name" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="mailTemplate" v-model="model" :meta="meta" :active-toggle="false" :disabled="disabled" />
  </ui-form>
</template>


<script>
  import MailTemplatesApi from 'zero/api/mailTemplates.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    props: ['id'],

    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { name: null },
      route: 'settings-mails-edit',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.id ? MailTemplatesApi.getEmpty() : MailTemplatesApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(MailTemplatesApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(MailTemplatesApi.delete.bind(this, this.id));
      }     
    }
  }
</script>
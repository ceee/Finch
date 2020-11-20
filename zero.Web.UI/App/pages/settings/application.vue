<template>
  <ui-form ref="form" class="application" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@application.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="application" v-model="model" :meta="meta" :disabled="disabled" />
  </ui-form>
</template>


<script>
  import ApplicationsApi from 'zero/api/applications.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    props: ['id'],

    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { name: null, features: [], domains: [] },
      route: zero.alias.settings.applications + '-edit',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        const id = this.$route.params.id;
        form.load(!id ? ApplicationsApi.getEmpty() : ApplicationsApi.getById(id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(ApplicationsApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(ApplicationsApi.delete.bind(this, this.$route.params.id));
      }
    }
  }
</script>
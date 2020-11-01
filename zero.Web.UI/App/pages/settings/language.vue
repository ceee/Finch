<template>
  <ui-form ref="form" class="language" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@language.name" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="language" v-model="model" :meta="meta" :disabled="disabled" />
  </ui-form>
</template>


<script>
  import LanguagesApi from 'zero/resources/languages.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    props: ['id'],

    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { name: null, features: [], domains: [] },
      route: zero.alias.settings.languages + '-edit',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.id ? LanguagesApi.getEmpty() : LanguagesApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(LanguagesApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(LanguagesApi.delete.bind(this, this.id));
      }     
    }
  }
</script>
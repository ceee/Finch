<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@country.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="country" v-model="model" :meta="meta" :disabled="disabled" />
  </ui-form>
</template>


<script>
  import CountriesApi from 'zero/resources/countries.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { name: null, features: [], domains: [] },
      route: zero.alias.settings.countries + '-edit',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.$route.params.id ? CountriesApi.getEmpty() : CountriesApi.getById(this.$route.params.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(CountriesApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(CountriesApi.delete.bind(this, this.$route.params.id));
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
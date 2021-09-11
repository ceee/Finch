<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" prefix="@country.list" title="@country.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="country" v-model="model" :meta="meta" :disabled="disabled">
      <template v-slot:below>
        <ui-editor-infos v-model="model" :disabled="disabled" />
      </template>
      <!--<template v-slot:aside>
        <ui-editor-aside v-model="model" :disabled="disabled" />
      </template>-->
    </ui-editor>
  </ui-form>
</template>


<script>
  import { countriesApi } from 'zerox';

  export default {
    props: ['id'],

    data: () => ({
      meta: {},
      model: { name: null, features: [], domains: [] },
      route: __zero.alias.settings.countries + '-edit',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.$route.params.id ? countriesApi.getEmpty() : countriesApi.getById(this.id, { scope: 'hallo' })).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(countriesApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(countriesApi.delete.bind(this, this.id));
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
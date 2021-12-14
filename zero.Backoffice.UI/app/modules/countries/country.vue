<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" prefix="@country.list" title="@country.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="country" v-model="model" :meta="meta" :disabled="disabled">
      <template v-slot:below>
        <ui-editor-infos v-model="model" :disabled="disabled" />
      </template>
    </ui-editor>
  </ui-form>
</template>

<!--<template>
  <ui-editor-form config="country" class="country" />
</template>-->


<script>
  import api from './api';

  export default {
    props: ['id'],

    data: () => ({
      meta: {},
      model: { name: null, features: [], domains: [] },
      route: 'countries-edit',
      disabled: false
    }),

    methods: {

      async onLoad(form)
      {
        const response = await form.load(() => api.getById(this.id));
        this.model = response;
      },


      onSubmit(form)
      {
        form.handle(api.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(api.delete.bind(this, this.id));
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
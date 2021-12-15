<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" prefix="@country.list" title="@country.name" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="countries:edit" v-model="model" :meta="meta" :disabled="disabled">
      <template v-slot:below>
        <ui-editor-infos v-model="model" :disabled="disabled" />
      </template>
    </ui-editor>
  </ui-form>
</template>


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
        const response = await form.load(() => this.id ? api.getById(this.id) : api.getEmpty(this.$route.query['zero.flavor']));
        this.model = response;
      },


      async onSubmit(form)
      {
        const response = this.id ? await api.update(this.model) : await api.create(this.model);
        await form.handle(response);
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
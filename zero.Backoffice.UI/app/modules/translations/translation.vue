<template>
  <ui-form ref="form" class="translation" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" prefix="@translation.list" title="@translation.name" :disabled="disabled" :is-create="!id" :state="form.state" :active-disabled="true" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="translations:edit" v-model="model" :meta="meta" :disabled="disabled" :scope="true">
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
      model: { },
      route: 'translations-edit',
      disabled: false
    }),

    methods: {

      async onLoad(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = await form.load(() => this.id ? api.getById(this.id, undefined, config) : api.getEmpty(this.$route.query['zero.flavor'], config));
        this.model = response;
      },


      async onSubmit(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = this.id ? await api.update(this.model, config) : await api.create(this.model, config);
        await form.handle(response);
      },


      onDelete(item, opts)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        opts.hide();
        this.$refs.form.onDelete(api.delete.bind(this, this.id, config));
      }
    }
  }
</script>
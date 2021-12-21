<template>
  <ui-form ref="form" class="language" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" prefix="@language.list" title="@language.name" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="languages:edit" v-model="model" :meta="meta" :disabled="disabled" :scope="true">
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
      route: 'languages-edit',
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
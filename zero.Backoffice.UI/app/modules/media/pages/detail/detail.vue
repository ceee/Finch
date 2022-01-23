<template>
  <ui-form ref="form" class="media-detail" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" prefix="@media.list" title="@media.name" :disabled="disabled" :active-disabled="true" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete">
      <template v-slot:actions>
        <ui-dropdown-button label="@media.actions.openfile" icon="fth-external-link" @click="openFile" :disabled="!id" :prevent="true" />
        <ui-dropdown-button label="@media.actions.replacefile" icon="fth-file-input" :disabled="true" />
      </template>
    </ui-form-header>
    <div class="media-detail-grid">
      <media-detail-preview :model="model" />
      <ui-editor config="media:edit" v-model="model" :meta="meta" :disabled="disabled" />
    </div>
  </ui-form>
</template>


<script>
  import api from '../../api';
  import MediaDetailPreview from './preview.vue';

  export default {
    props: ['id'],

    data: () => ({
      meta: {},
      model: { name: null },
      route: 'media-edit',
      disabled: false
    }),

    components: { MediaDetailPreview },

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
      },

      async openFile(val, { dropdown, hide, loading })
      {
        loading(true);
        const result = await api.getSource(this.id);
        let path = result.success ? result.data.path : null;
        loading(false);
        hide();

        if (path)
        {
          window.open(window.location.origin + path, 'blank');
        }
      }
    }
  }
</script>


<style lang="scss">
  .media-detail-grid
  {
    display: grid;
    grid-template-columns: minmax(auto, 1fr) 420px;
    grid-gap: var(--padding-xs);
    max-width: 1160px;
    margin: 0 auto;
    padding: 0 var(--padding);

    .editor
    {
      padding: 0;
    }
  }

  .media-detail-center
  {
    display: grid;
    grid-template-rows: auto minmax(auto, 1fr);
    grid-gap: var(--padding-xs);
  }
</style>
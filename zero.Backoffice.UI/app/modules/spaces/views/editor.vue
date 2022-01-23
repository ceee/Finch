<template>
  <ui-form :key="id + alias" ref="form" class="space-editor" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" :title="space.name" :title-disabled="space.view === 'editor'" :disabled="disabled" :is-create="isCreate" :state="form.state" 
                    :can-delete="space.view != 'editor' && meta.canDelete" @delete="onDelete" :active-toggle="true" />
    <ui-editor v-if="editor" :config="editor" v-model="model" :meta="meta" :disabled="disabled" :active-toggle="false">
      <template v-slot:below>
        <ui-editor-infos v-model="model" :disabled="disabled" />
      </template>
    </ui-editor>
  </ui-form>
</template>


<script>
  import api from '../api';
  import { defineComponent } from 'vue';
  //import Overlay from 'zero/helpers/overlay.js';

  export default defineComponent({

    props: ['id', 'alias'],

    data: () => ({
      disabled: false,
      editor: null,
      meta: {},
      route: null, //zero.alias.sections.settings + '-' + zero.alias.settings.countries + '-edit',
      space: {},
      model: { name: null }
    }),

    computed: {
      isCreate()
      {
        return this.id === 'create' || (this.space.view == 'editor' && this.model && !this.model.id);
      }
    },

    methods: {

      async setup()
      {
        const result = await api.getTypes();

        this.space = result.data.find(x => x.alias == this.alias);

        if (!this.space)
        {
          // TODO v3 error
        }

        let alias = this.space.editorAlias || (this.space.alias + ':edit');
        alias = alias.indexOf('spaces:') === 0 ? alias : 'spaces:' + alias;
        this.editor = await this.zero.getSchema(alias);
      },


      async onLoad(form)
      {
        this.editor = null;

        await this.setup();

        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = await form.load(() =>
        {
          if (this.space.view == 'editor')
          {
            return api.getByAlias(this.space.alias);
          }
          else if (!this.isCreate)
          {
            return api.getById(this.space.alias, this.id, undefined, config);
          }

          return api.getEmpty(this.space.alias, config)
        });

        this.model = response;
      },


      async onSubmit(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = !this.isCreate ? await api.update(this.model, config) : await api.create(this.model, config);
        await form.handle(response);

        if (response.success)
        {
          this.model = response.data;

          if (this.id === 'create')
          {
            this.$router.replace({ name: 'spaces-list-edit', params: { alias: this.alias, id: this.model.id } });
          }
        }
      },


      onDelete(item, opts)
      {
        opts.hide();
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        this.$refs.form.onDelete(api.delete.bind(this, this.id, config));
      }

    }
  })
</script>

<style lang="scss">
  .space-editor .ui-header-bar + .editor > .ui-box
  {
    margin-top: 0;
  }

  .space-editor .editor-outer.-infos-aside
  {
    grid-template-columns: 1fr;

    .editor-infos
    {
      margin: -31px var(--padding) 0;
    }
  }
</style>
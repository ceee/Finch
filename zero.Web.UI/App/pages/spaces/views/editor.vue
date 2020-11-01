<template>
  <ui-form ref="form" class="space-editor" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" :title="space.name" :title-disabled="space.view === 'editor'" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" :active-toggle="true" />
    <ui-editor v-if="editor" :config="editor" v-model="model" :meta="meta" :disabled="disabled" :active-toggle="false" />
  </ui-form>
</template>


<script>
  import SpacesApi from 'zero/resources/spaces.js';
  import UiEditor from 'zero/editor/editor.vue';
  import Overlay from 'zero/services/overlay.js';

  export default {
    props: ['config', 'space'],

    components: { UiEditor },

    data: () => ({
      disabled: false,
      editor: null,
      meta: {},
      route: null, //zero.alias.sections.settings + '-' + zero.alias.settings.countries + '-edit',
      model: { name: null }
    }),

    computed: {
      id()
      {
        return this.$route.params.id;
      },
      alias()
      {
        return this.$route.params.alias;
      },
      isList()
      {
        return !!this.id;
      }
    },

    watch: {
      '$route': 'setup'
    },

    methods: {

      setup()
      {
        this.editor = this.zero.getEditor('spaces.' + this.space.alias);
      },

      onLoad(form)
      {
        this.setup();

        form.load(SpacesApi.getContent(this.alias, this.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;         
          this.meta = response.meta;
          this.model = response.entity;
          this.route = { name: 'space-item', params: { alias: this.alias } };
        });
      },


      onSubmit(form)
      {
        form.handle(SpacesApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(SpacesApi.delete.bind(this, this.alias, this.id));
      }  
    }
  }
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
<template>
  <ui-form ref="form" v-slot="form" @submit="onSubmit" class="editor-form">
    <ui-form-header v-if="rendered" v-model="model" prefix="@country.list" title="@country.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor v-if="rendered" :config="config" v-model="model" :meta="meta" :disabled="disabled" :on-configure="onLoad">
      <template v-slot:below>
        <ui-editor-infos v-model="model" :disabled="disabled" />
      </template>
    </ui-editor>
  </ui-form>
</template>


<script>
  import './editor.scss';

  export default {
    name: 'uiEditorForm',

    props: {
      config: {
        type: [String, Object],
        required: true
      }
    },

    data: () => ({
      meta: {},
      model: {},
      route: __zero.alias.settings.countries + '-edit',
      disabled: false,
      collection: null,
      rendered: false
    }),


    mounted()
    {
      this.rendered = true;
    },


    methods: {

      onLoad(editor)
      {
        this.collection = editor.editorConfig.collection;

        if (!this.collection)
        {
          // TODO error
        }

        //this.$refs.form.load(!this.$route.params.id ? this.collection.getEmpty() : this.collection.getById(this.$route.params.id)).then(response =>
        //{
        //  this.disabled = !response.meta.canEdit;
        //  this.meta = response.meta;
        //  this.model = response.entity;
        //});
      },


      onSubmit(form)
      {
        form.handle(this.collection.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(this.collection.delete.bind(this, this.$route.params.id));
      }
    }
  }
</script>
<template>
  <ui-form ref="form" class="editor-page" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model:value="model" :prefix="prefix" :title="title" :disabled="readonly" :is-create="!id" :state="form.state" :can-delete="canDelete" @delete="onDelete" />
    <ui-editor :config="editor" v-model="model" :meta="meta" :disabled="readonly" :scope="true">
      <template v-slot:below>
        <ui-editor-infos v-model="model" :disabled="readonly" />
      </template>
    </ui-editor>
  </ui-form>
</template>


<script>
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'uiEditorPage',

    props: {
      api: {
        type: Object,
        required: true
      },
      editor: {
        type: String,
        required: true
      },
      title: {
        type: String,
        required: true
      },
      id: {
        type: String,
        required: false
      },
      prefix: {
        type: String,
        required: false
      },
      route: {
        type: String,
        required: false
      },
      canDelete: {
        type: Boolean,
        default: true
      },
      disabled: {
        type: [Boolean, Function],
        default: false
      },
    },


    computed: {
      id()
      {
        return this.$route.params.id;
      },
      readonly()
      {
        return typeof this.disabled === 'function' ? this.disabled(this.model) : this.disabled;
      }
    },


    data: () => ({
      meta: {},
      model: {}
    }),

    methods: {

      async onLoad(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = await form.load(() => this.id ? this.api.getById(this.id, undefined, config) : this.api.getEmpty(this.$route.query['zero.flavor'], config));
        this.model = response;
      },


      async onSubmit(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        const response = this.id ? await this.api.update(this.model, config) : await this.api.create(this.model, config);
        await form.handle(response);
      },


      onDelete(item, opts)
      {
        opts.hide();
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        this.$refs.form.onDelete(this.api.delete.bind(this, this.id, config));
      }
    }
  })
</script>
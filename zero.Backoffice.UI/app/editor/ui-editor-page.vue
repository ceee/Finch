<template>
  <ui-form ref="form" class="editor-page" v-slot="form" @submit="onInternalSubmit" @load="onInternalLoad" :route="route">
    <ui-form-header v-model:value="model" :prefix="prefix" :title="title" :disabled="readonly" :is-create="!id" :state="form.state" :can-delete="canDelete" @delete="onInternalDelete" />
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
      onDelete: {
        type: Function,
        default: null
      },
      onLoad: {
        type: Function,
        default: null
      },
      onSubmit: {
        type: Function,
        default: null
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

      async onInternalLoad(form)
      {
        var config = { system: this.$route.query['zero.scope'] == 'system' };

        if (typeof this.onLoad === 'function')
        {
          this.model = await this.onLoad({ id: this.id, api: this.api, form, config});
        }
        else
        {
          const response = await form.load(() => this.id ? this.api.getById(this.id, undefined, config) : this.api.getEmpty(this.$route.query['zero.flavor'], config));
          this.model = response;
        }
        
      },


      async onInternalSubmit(form)
      {
        form.setState('loading');

        let isCreate = !this.id;
        var config = { system: this.$route.query['zero.scope'] == 'system' };
        let response = null;

        if (typeof this.onSubmit === 'function')
        {
          response = await this.onSubmit({ id: this.id, isCreate, config, form, model: this.model });
        }
        else
        {
          response = !isCreate ? await this.api.update(this.model, config) : await this.api.create(this.model, config);
        }

        await form.handle(response);

        if (response.success)
        {
          if (isCreate)
          {
            this.$emit('resource-created', response.data);
          }
          this.$emit('resource-updated', response.data);
          this.model = response.data;
        }
      },


      onInternalDelete(item, opts)
      {
        opts.hide();
        if (typeof this.onDelete === 'function')
        {
          this.onDelete(this.model, opts);
        }
        else
        {
          var config = { system: this.$route.query['zero.scope'] == 'system' };
          this.$refs.form.onDelete(this.api.delete.bind(this, this.id, config));
        }
      }
    }
  })
</script>
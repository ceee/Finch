<template>
  <ui-form ref="form" class="mediaitem" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@media.name" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="media" v-model="model" :meta="meta" :active-toggle="false" :disabled="disabled">
      <template v-slot:infos-more>
        <ui-property v-if="model.imageMeta" label="@media.fields.date" :is-text="true">
          <ui-date v-model="model.imageMeta.createdDate" />
        </ui-property>
        <ui-property label="@media.fields.size" :is-text="true">
          <span v-filesize="model.size"></span>
        </ui-property>
      </template>
      <template v-slot:infos-after v-if="model.imageMeta">
        <div class="ui-box">
          <ui-property v-if="model.imageMeta.width" label="@media.fields.dimension" :is-text="true">
            {{model.imageMeta.width}} × {{model.imageMeta.height}}
          </ui-property>
          <ui-property v-if="model.imageMeta.dpi != 0" label="@media.fields.dpi" :is-text="true">
            {{model.imageMeta.dpi}}
          </ui-property>
          <ui-property v-if="model.imageMeta.colorSpace" label="@media.fields.colorSpace" :is-text="true">
            {{model.imageMeta.colorSpace}}
          </ui-property>
          <ui-property v-if="model.imageMeta.frames > 1" label="@media.fields.frames" :is-text="true">
            {{model.imageMeta.frames}}
          </ui-property>
        </div>
      </template>
    </ui-editor>
  </ui-form>
</template>


<script>
  import MediaApi from 'zero/resources/media.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    props: ['id'],

    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { },
      route: 'mediaitem',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.id ? MediaApi.getEmpty() : MediaApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(MediaApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(MediaApi.delete.bind(this, this.id));
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
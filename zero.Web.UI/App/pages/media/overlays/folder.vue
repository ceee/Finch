<template>
  <ui-form v-if="!loading" ref="form" class="mediafolder" v-slot="form" @submit="onSubmit" @load="onLoad">
    <h2 class="ui-headline" v-localize="model.id ? '@media.editfolder' : '@media.addfolder'"></h2>
    <div class="mediafolder-items">
      <input v-model="item.name" type="text" class="ui-input" maxlength="200" v-localize:placeholder="'@media.fields.foldername_placeholder'" :readonly="disabled" />
      <!-- // TODO add parent selector -->
    </div>
    <div class="app-confirm-buttons">
      <ui-button type="primary" v-if="!disabled" :submit="true" :state="form.state" :label="model.id ? '@ui.save' : '@ui.create'"></ui-button>
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
      <ui-button v-if="!disabled && model.id" type="light" label="@ui.delete" @click="onDelete" style="float:right;"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  import MediaFolderApi from 'zero/resources/media-folder.js';
  import Overlay from 'zero/services/overlay.js';

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      loading: false,
      item: {},
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.model.id ? MediaFolderApi.getEmpty() : MediaFolderApi.getById(this.model.id)).then(response =>
        {
          this.disabled = false; //!response.canEdit;
          this.item = response.entity;
          this.item.parentId = this.model.parentId;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        form.handle(MediaFolderApi.save(this.item)).then(response =>
        {
          this.config.confirm(response, this.config);
        });
      },

      onDelete()
      {
        this.config.confirm({ deleted: true });
      }

    }
  }
</script>

<style lang="scss">
  .mediafolder
  {
    text-align: left;
  }

  .mediafolder-items
  {
    margin-top: var(--padding);
  }
</style>
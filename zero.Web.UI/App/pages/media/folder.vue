<template>
  <ui-form v-if="!loading" ref="form" class="mediafolder" v-slot="form" @submit="onSubmit" @load="onLoad">
    <h2 class="ui-headline" v-localize="'@media.name'"></h2>
    <div class="mediafolder-items">
      <ui-property label="@ui.name" :required="true" :vertical="true">
        <input v-model="item.name" type="text" class="ui-input" maxlength="200" :readonly="disabled" />
      </ui-property>
        <!-- // TODO add parent selector -->
    </div>
    <div class="app-confirm-buttons">
      <ui-button v-if="!disabled" :submit="true" :state="form.state" label="@ui.save"></ui-button>
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
        console.info(JSON.parse(JSON.stringify(this.model)));

        form.load(!this.model.id ? MediaFolderApi.getEmpty() : MediaFolderApi.getById(this.model.id)).then(response =>
        {
          this.disabled = !response.canEdit;
          this.item = response;
          this.item.parentId = this.model.parentId;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        form.handle(MediaFolderApi.save(this.item)).then(response =>
        {
          this.config.confirm(response);
        });
      },

      onDelete()
      {
        Overlay.confirmDelete().then((opts) =>
        {
          opts.state('loading');

          MediaFolderApi.delete(this.model.id).then(response =>
          {
            if (response.success)
            {
              opts.state('success');
              opts.hide();
              this.config.close();
            }
            else
            {
              opts.errors(response.errors);
            }
          });
        }); 
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

    .ui-property + .ui-property,
    .ui-split + .ui-property
    {
      margin-top: 0;
    }
  }
</style>
<template>
  <ui-form v-if="!loading" ref="form" class="translation" v-slot="form" @submit="onSubmit" @load="onLoad">
    <h2 class="ui-headline" v-localize="'@translation.name'"></h2>
    <div class="translation-items">
      <div class="ui-split">
        <ui-property label="@translation.fields.key" :required="true" :vertical="true">
          <input v-model="item.key" type="text" class="ui-input" maxlength="200" :readonly="disabled" />
        </ui-property>
        <ui-property label="@translation.fields.display" :vertical="true">
          <ui-state-button :disabled="disabled" :items="displayItems" v-model="item.display" />
        </ui-property>
      </div>
      <ui-property label="@translation.fields.value" :required="true" :vertical="true">
        <textarea v-model="item.value" class="ui-input" :readonly="disabled"></textarea>
      </ui-property>
    </div>
    <div class="app-confirm-buttons">
      <ui-button v-if="!disabled" :submit="true" :state="form.state" label="@ui.save"></ui-button>
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
      <ui-button v-if="!disabled && model.id" type="light" label="@ui.delete" @click="onDelete" style="float:right;"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  import TranslationsApi from 'zero/resources/translations.js';
  import Overlay from 'zero/services/overlay.js';

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      loading: false,
      item: {},
      disabled: false,
      displayItems: [
        { label: '@translation.display.text', value: 'text' },
        { label: '@translation.display.html', value: 'html' }
      ]
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.model.id ? TranslationsApi.getEmpty() : TranslationsApi.getById(this.model.id)).then(response =>
        {
          this.disabled = !response.canEdit;
          this.item = response;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        form.handle(TranslationsApi.save(this.item)).then(response =>
        {
          console.info(response);
        });
      },

      onDelete()
      {
        Overlay.confirmDelete().then((opts) =>
        {
          opts.state('loading');

          TranslationsApi.delete(this.model.id).then(response =>
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
  .translation
  {
    text-align: left;
  }

  .translation-items
  {
    margin-top: var(--padding);

    .ui-property + .ui-property,
    .ui-split + .ui-property
    {
      margin-top: 0;
    }
  }
</style>
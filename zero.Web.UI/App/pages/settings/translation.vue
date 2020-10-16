<template>
  <ui-form v-if="!loading" ref="form" class="translation" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <h2 class="ui-headline" v-localize="'@translation.name'"></h2>
    <div class="translation-items">
      <div v-if="form.isShared" class="editor-global-flag is-block">
        <b>This entity is shared</b> and can be used by all applications.<br>
        <a href="/">More info</a>
        <i class="fth-radio"></i>
      </div>
      <div class="ui-split">
        <ui-property label="@translation.fields.key" :required="true" :vertical="true" field="key">
          <input v-model="item.key" type="text" class="ui-input" maxlength="300" :disabled="disabled" />
        </ui-property>
        <ui-property label="@translation.fields.display" :vertical="true" field="display">
          <ui-state-button :disabled="disabled" :items="displayItems" v-model="item.display" />
        </ui-property>
      </div>
      <br />
      <ui-property label="@translation.fields.value" :required="true" :vertical="true" field="value">
        <textarea v-if="item.display === 'text'" v-model="item.value" class="ui-input" :disabled="disabled"></textarea>
        <ui-rte v-if="item.display === 'html'" v-model="item.value" :disabled="disabled" />
      </ui-property>
    </div>
    <div class="app-confirm-buttons">
      <ui-button type="primary" v-if="!disabled" :submit="true" :state="form.state" label="@ui.save"></ui-button>
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
      <ui-button type="light" v-if="!disabled && model.id" label="@ui.delete" @click="onDelete" style="float:right;"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  import TranslationsApi from 'zero/resources/translations.js';
  import Overlay from 'zero/services/overlay.js';
  import Editor from 'zero/editor/editor';

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      loading: false,
      meta: {},
      item: { key: null, value: null },
      disabled: false,
      route: zero.alias.settings.translations + '-edit',
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
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.item = response.entity;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        form.handle(TranslationsApi.save(this.item)).then(res =>
        {
          this.config.confirm(res);
        });
      },


      onDelete()
      {
        this.$refs.form.onDelete(TranslationsApi.delete.bind(this, this.item.id));
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
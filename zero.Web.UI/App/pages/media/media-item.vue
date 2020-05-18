<template>
  <ui-form v-if="!loading" ref="form" class="ui-media-overlay" v-slot="form" @submit="onSubmit">
    <h2 class="ui-headline" v-localize="'@media.name'"></h2>

    <div class="ui-media-overlay-preview">
      <img class="ui-media-overlay-preview-image" v-if="model.source" :src="model.source" :alt="model.name" />
    </div>

    <div class="ui-media-overlay-infos ui-split-three">
      <ui-property label="@media.fields.size" :vertical="true" :is-text="true">
        <span v-filesize="model.size"></span>
      </ui-property>
      <ui-property v-if="model.lastModifiedDate" label="@media.fields.date" :vertical="true" :is-text="true">
        <ui-date v-model="model.lastModifiedDate" />
      </ui-property>
      <ui-property v-if="model.dimension" label="@media.fields.dimension" :vertical="true" :is-text="true">
        {{model.dimension.width}} × {{model.dimension.height}}
      </ui-property>
    </div>

    <div class="ui-media-overlay-fields">
      <ui-property label="@ui.name" :required="true" :vertical="true" :is-text="true">
        <input v-model="model.name" type="text" class="ui-input" maxlength="160" :readonly="true" />
      </ui-property>
      <ui-property label="@media.fields.alternativeText" :vertical="true">
        <input v-model="model.alternativeText" type="text" class="ui-input" maxlength="160" :readonly="disabled" />
      </ui-property>
      <ui-property label="@media.fields.caption" :vertical="true">
        <textarea v-model="model.caption" class="ui-input" :readonly="disabled"></textarea>
      </ui-property>
    </div>

    <div class="app-confirm-buttons">
      <ui-button v-if="!disabled" type="action" :submit="true" :state="form.state" label="@ui.save"></ui-button>
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
      <ui-button v-if="!disabled && !isCreate" type="light" label="@ui.remove" @click="onDelete" style="float:right;"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  import Overlay from 'zero/services/overlay.js';

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      loading: false
    }),

    computed: {
      disabled()
      {
        return this.config.disabled;
      },
      isCreate()
      {
        return this.config.isCreate;
      }
    },

    methods: {

      onSubmit()
      {
         this.config.confirm(this.model);
      },

      onDelete()
      {
        this.config.confirm({
          deletionRequested: true
        });
      }

    }
  }
</script>

<style lang="scss">
  .ui-media-overlay
  {
    text-align: left;
  }

  .ui-media-overlay-preview
  {
    height: 200px;
    background: var(--color-bg-mid);
    border-radius: var(--radius) var(--radius) 0 0;
    margin-top: var(--padding);
    padding: 10px;
    overflow: hidden;
  }

  .ui-media-overlay-preview-image
  {
      border-radius: 3px;
      max-width: 100%;
      max-height: 100%;
      margin: auto;
      display: block;
      color: transprent;
      overflow: hidden;
      font-size: 0;
      position: relative;
      z-index: 2;
  }

  .ui-media-overlay-infos
  {
    background: var(--color-bg-mid);
    border-radius: 0 0 var(--radius) var(--radius);
    padding: 15px 20px;

    .ui-property-content
    {
      color: var(--color-fg-mid);   
    }
  }

  .ui-media-overlay-fields
  {
    margin-top: var(--padding);

    .ui-property + .ui-property, .ui-split + .ui-property
    {
      margin-top: 0;
    }
  }

  /*.translation-items
  {
    margin-top: var(--padding);

    .ui-property + .ui-property,
    .ui-split + .ui-property
    {
      margin-top: 0;
    }
  }*/
</style>
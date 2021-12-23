<template>
  <div class="media-detail-metadata">
    <ui-property label="@media.fields.size" :vertical="false">
      <span v-filesize="config.model.size"></span>
    </ui-property>
    <ui-property label="@ui.createdDate" field="createdDate" :vertical="false">
      <ui-date :value="config.model.createdDate" />
    </ui-property>
    <ui-property v-if="config.model.lastModifiedDate" :vertical="false" label="@ui.modifiedDate">
      <ui-date :value="config.model.lastModifiedDate" />
    </ui-property>
    <template v-if="config.model.imageMeta">
      <ui-property v-if="config.model.imageMeta.width" label="@media.fields.imageMeta.dimension" :is-text="true" :vertical="false">
        {{config.model.imageMeta.width}} × {{config.model.imageMeta.height}}
      </ui-property>
      <ui-property v-if="config.model.imageMeta.dpi != 0" label="@media.fields.imageMeta.dpi" :is-text="true" :vertical="false">
        {{config.model.imageMeta.dpi}}
      </ui-property>
      <ui-property v-if="config.model.imageMeta.colorSpace" label="@media.fields.imageMeta.colorSpace" :is-text="true" :vertical="false">
        {{config.model.imageMeta.colorSpace}}
      </ui-property>
      <ui-property v-if="config.model.imageMeta.frames > 1" label="@media.fields.imageMeta.frames" :is-text="true" :vertical="false">
        {{config.model.imageMeta.frames}}
      </ui-property>
    </template>
  </div>
</template>

<script>
  export default {
    props: {
      value: Object,
      config: Object,
    }
  }
</script>

<style lang="scss">
  .media-detail
  {
    .ui-property + .ui-property
    {
      margin-top: 15px;
      padding-top: 15px;
      border-top: none;
    }
  }

  .media-detail-metadata
  {
    .ui-property + .ui-property
    {
      margin-top: var(--padding-xs);
      padding-top: 0;
      border-top: none;
    }
  }
</style>
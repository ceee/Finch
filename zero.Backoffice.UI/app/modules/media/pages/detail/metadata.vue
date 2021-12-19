<template>
  <div class="media-detail-metadata">
    <ui-property label="@media.fields.size" :vertical="false">
      <span v-filesize="entity.size"></span>
    </ui-property>
    <ui-property label="@ui.createdDate" field="createdDate" :vertical="false">
      <ui-date :value="entity.createdDate" />
    </ui-property>
    <ui-property v-if="entity.lastModifiedDate" :vertical="false" label="@ui.modifiedDate">
      <ui-date :value="entity.lastModifiedDate" />
    </ui-property>
    <template v-if="entity.imageMeta">
      <ui-property v-if="entity.imageMeta.width" label="@media.fields.imageMeta.dimension" :is-text="true" :vertical="false">
        {{entity.imageMeta.width}} × {{entity.imageMeta.height}}
      </ui-property>
      <ui-property v-if="entity.imageMeta.dpi != 0" label="@media.fields.imageMeta.dpi" :is-text="true" :vertical="false">
        {{entity.imageMeta.dpi}}
      </ui-property>
      <ui-property v-if="entity.imageMeta.colorSpace" label="@media.fields.imageMeta.colorSpace" :is-text="true" :vertical="false">
        {{entity.imageMeta.colorSpace}}
      </ui-property>
      <ui-property v-if="entity.imageMeta.frames > 1" label="@media.fields.imageMeta.frames" :is-text="true" :vertical="false">
        {{entity.imageMeta.frames}}
      </ui-property>
    </template>
  </div>
</template>

<script>
  export default {
    props: {
      value: Object,
      entity: Object
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
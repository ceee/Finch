<template>
  <div class="ui-module-preview-tags">
    <div class="ui-module-preview-tag" v-for="item in items">{{(!!property ? item[property] : item)}}</div>
  </div>
</template>

<script>
  export default {
    name: 'uiModulePreviewTags',

    props: {
      items: {
        type: Array
      },
      property: {
        type: String,
        default: null
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-preview-tags
  {
    display: flex;
    gap: 8px;
  }

  .ui-module-preview-tag
  {
    display: inline-block;
    border: 1px solid var(--color-line);
    border-radius: var(--radius);
    padding: 6px 8px 4px;
  }
</style>
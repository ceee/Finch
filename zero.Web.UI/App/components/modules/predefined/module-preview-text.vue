<template>
  <div class="ui-module-preview-text">
    <article class="-text" v-if="text" v-html="text"></article>
    <article class="-subline" v-if="subline" v-html="subline"></article>
  </div>
</template>

<script>
  export default {
    name: 'uiModulePreviewText',

    props: {
      text: {
        type: String
      },
      subline: {
        type: String
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-preview-text
  {
    font-size: var(--font-size);
    line-height: 1.5;

    .-text
    {

    }

    .-subline
    {
      color: var(--color-text-dim);
      font-size: var(--font-size-s);
    }

    .-text, .-subline
    {
      overflow: hidden;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 3;
      display: -webkit-box;
    }
  }
</style>
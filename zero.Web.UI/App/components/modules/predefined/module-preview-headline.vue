<template>
  <div class="ui-module-preview-headline">
    <article class="-text" v-if="text" v-html="text"></article>
    <article class="-subline" v-if="subline" v-html="subline"></article>
  </div>
</template>

<script>
  export default {
    name: 'uiModulePreviewHeadline',

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
  .ui-module-preview-headline
  {
    font-size: var(--font-size);
    line-height: 1.5;

    .-text
    {
      font-size: var(--font-size-xl);
      font-weight: 600;
    }

    .-subline
    {
      color: var(--color-fg-dim);
      font-size: var(--font-size-s);
    }
  }
</style>
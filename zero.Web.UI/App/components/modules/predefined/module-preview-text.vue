<template>
  <div class="ui-module-preview-text">
    <p class="-text" v-if="text" v-html="textContent" :style="{ '-webkit-line-clamp': maxLines }"></p>
    <p class="-subline" v-if="subline" v-html="sublineContent"></p>
  </div>
</template>

<script>
  import Strings from 'zero/helpers/strings.js';

  export default {
    name: 'uiModulePreviewText',

    props: {
      text: {
        type: String
      },
      subline: {
        type: String
      },
      maxLines: {
        type: Number,
        default: 6
      },
      html: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      textContent()
      {
        return this.html ? this.text : Strings.htmlToText(this.text, true);
      },
      sublineContent()
      {
        return this.html ? this.subline : Strings.htmlToText(this.subline);
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-preview-text
  {
    font-size: var(--font-size);
    line-height: 1.5;

    .-text, .-subline
    {
      overflow: hidden;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 3;
      display: -webkit-box;
    }

    p.-subline
    {
      color: var(--color-text-dim);
      font-size: var(--font-size-s);
      margin-top: 0.2em;
    }
  }
</style>
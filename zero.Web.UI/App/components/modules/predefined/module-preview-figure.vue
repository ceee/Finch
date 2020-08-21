<template>
  <figure class="ui-module-preview-figure" :class="{'has-image': imageSource != null}">
    <img v-if="imageSource" :src="imageSource" class="-image" />
    <figcaption>
      <article class="-text" v-if="text" v-html="text"></article>
      <article class="-subline" v-if="subline">{{subline.replace(/<\/?[^>]+>/ig, " ")}}</article>
    </figcaption>
  </figure>
</template>

<script>
  import MediaApi from 'zero/resources/media.js'


  export default {
    name: 'uiModulePreviewFigure',

    props: {
      text: {
        type: String
      },
      subline: {
        type: String
      },
      image: {
        type: String
      }
    },

    data: () => ({
      imageSource: null
    }),

    watch: {
      image(val)
      {
        this.loadImageSource();
      }
    },

    mounted()
    {
      this.loadImageSource();
    }, 

    methods: {

      loadImageSource()
      {
        if (!this.image)
        {
          this.imageSource = null;
          return;
        }

        this.imageSource = MediaApi.getImageSource(this.image);
      }
    }
  }
</script>

<style lang="scss">
  .ui-module-preview-figure
  {
    font-size: var(--font-size);
    line-height: 1.5;
    margin: 0;
    padding: 0;

    &.has-image
    {
      display: grid;
      grid-template-columns: auto 1fr;
      grid-gap: 20px;
      align-items: center;
    }

    .-text
    {

    }

    .-subline
    {
      color: var(--color-fg-dim);
      font-size: var(--font-size-s);
    }

    .-text, .-subline
    {
      overflow: hidden;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 2;
      display: -webkit-box;
    }

    .-image
    {
      border-radius: var(--radius);
      max-width: 128px;
      max-height: 64px;
    }
  }
</style>
<template>
  <figure class="ui-module-preview-figure" :class="{'has-image': imageSource != null}">
    <img v-if="imageSource" :src="imageSource" class="-image" />
    <figcaption>
      <p>
        <span class="-text" v-if="text" v-html="textContent"></span>
        <span class="-subline" v-if="subline" v-html="sublineContent"></span>
      </p>
    </figcaption>
  </figure>
</template>

<script>
  import MediaApi from 'zero/api/media.js';
  import Strings from 'zero/helpers/strings.js';


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
      },
      imageSize: {
        type: String,
        default: 'thumbnail'
      },
      html: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      imageSource: null
    }),

    computed: {
      textContent()
      {
        return this.html ? this.text : Strings.htmlToText(this.text, true);
      },
      sublineContent()
      {
        return this.html ? this.subline : Strings.htmlToText(this.subline);
      }
    },

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

        this.imageSource = MediaApi.getImageSource(this.image, this.imageSize);
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
      gap: 20px;
      align-items: center;
    }

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

    .-image
    {
      border-radius: var(--radius-inner);
      max-width: 218px;
      max-height: 124px;
    }
  }
</style>
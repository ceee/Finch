<template>
  <div class="media-detail-file media-pattern">
    <div v-if="model.imageMeta" ref="image" class="media-detail-file-image" @click="setFocalPoint" @dblclick="model.imageMeta.focalPoint = null">
      <ui-thumbnail v-if="model.id" :media="model.id" size="preview" />
      <span class="media-detail-file-focal-point" :style="getFocalPointStyle(model.imageMeta.focalPoint)"><span class="-dot"></span></span>
    </div>
    <div v-else class="media-detail-file-generic">
      <ui-icon symbol="fth-file-text" :size="64" :stroke="1" />
      <p>
        <span v-localize="extension"></span><br /><span v-filesize="model.size"></span>
      </p>
    </div>
  </div>
</template>

<script lang="ts">
  //import * as overlays from '../../../../services/overlay';

  export default {
    name: 'mediaDetailPreview',

    props: {
      model: Object,
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      
    }),


    computed: {
      extension()
      {
        if (!this.model || !this.model.path)
        {
          return '@media.untitledName';
        }
        const parts = this.model.path.split('.');
        return parts[parts.length - 1].toUpperCase();
      }
    },


    methods: {

      getFocalPointStyle(point)
      {
        return {
          display: 'inline-block',
          left: (point ? point.left : 0.5) * 100 + '%',
          top: (point ? point.top : 0.5) * 100 + '%'
        }
      },

      setFocalPoint(ev)
      {
        if (this.disabled)
        {
          return;
        }

        let image = this.$refs.image.getBoundingClientRect();
        let point = { x: ev.pageX - image.x, y: ev.pageY - image.y };
        const left = +(point.x / image.width).toFixed(2);
        const top = +(point.y / image.height).toFixed(2);

        this.model.imageMeta.focalPoint = {
          left: left < 0 ? 0 : (left > 1 ? 1 : left),
          top: top < 0 ? 0 : (top > 1 ? 1 : top)
        };
      }

    }
  };
</script>

<style lang="scss">
  .media-detail-file
  {
    display: flex;
    justify-content: center;
    align-items: center;
    padding: var(--padding);
    border-radius: var(--radius);

    img
    {
      border-radius: var(--radius);
      max-height: 550px;
      z-index: 1;
    }

    .ui-icon
    {
      z-index: 1;
      color: var(--color-text);
    }
  }

  .media-detail-file-image
  {
    display: inline-block;
    border-radius: var(--radius);
    position: relative;
    overflow: hidden;
  }

  .media-detail-file-generic
  {
    position: relative;
    z-index: 1;
    text-align: center;

    p
    {
      margin-top: 0.8em;
      font-size: var(--font-size-s);
      color: var(--color-text-dim);
      line-height: 1.5;

      &:first-line
      {
        font-weight: 700;
        font-size: var(--font-size);
        color: var(--color-text);
      }
    }
  }

  .media-detail-file-focal-point
  {
    display: none;
    position: absolute;
    left: 50%;
    top: 50%;
    margin: -9px 0 0 -9px;
    width: 16px;
    height: 16px;
    z-index: 2;
    //mix-blend-mode: soft-light;
    opacity: 0.7;

    .-dot
    {
      display: block;
      width: 100%;
      height: 100%;
      border-radius: 20px;
      background: white;
      border: 1px solid #333;
      position: relative;
      z-index: 1;
    }

    &:before, &:after
    {
      content: '';
      position: absolute;
      background: black;
      opacity: 0.4;
      //mix-blend-mode: soft-light;
    }

    &:before
    {
      top: 50%;
      left: -550px;
      width: 1100px;
      height: 1px;
    }

    &:after
    {
      left: 50%;
      top: -550px;
      height: 1100px;
      width: 1px;
    }
  }

  /*.media-detail-file-image:hover .media-detail-file-focal-point
  {
    &:before, &:after
    {
      opacity: 1;
    }
  }*/
</style>

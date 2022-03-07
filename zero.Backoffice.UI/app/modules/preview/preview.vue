<template>
  <div class="app-preview theme-light">
    <iframe class="app-preview-frame" :src="src" />
    <div class="app-preview-overlay theme-dark">
      <span class="-circle"></span>
      <span class="-text">Preview</span>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from './api';

  export default defineComponent({

    data: () => ({
      src: null
    }),

    created()
    {
      api.createPreviewToken('mykey').then(res =>
      {
        let model = res.data;
        this.src = 'http://localhost:2310' + this.$route.query.path + `?${model.queryParameter}=${model.token}`;
      });
    }
  });
</script>

<style lang="scss">

  .app.is-preview
  {
    display: block;
    overflow: hidden;
  }

  .app-preview
  {
    width: 100%;
    height: 100%;
    position: relative;
    background: var(--color-bg);

    .-text
    {
      color: var(--color-text);
    }
  }

  iframe.app-preview-frame
  {
    width: 100%;
    height: 100%;
    margin: 0;
    padding: 0;
    border: none;
    position: relative;
    z-index: 0;
  }

  .app-preview-overlay
  {
    height: 32px;
    border-radius: 0 0 var(--radius) var(--radius);
    background: var(--color-bg);
    position: fixed;
    top: 0;
    left: 50%;
    transform: translateX(-50%);
    padding: 0 var(--padding-s);
    z-index: 1;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: 600;
    font-size: var(--font-size);

    .-circle
    {
      position: relative;
      top: -1px;
      width: 12px;
      height: 12px;
      border-radius: 16px;
      background: var(--color-accent);
      animation: shift 1.2s linear reverse infinite;
      margin-right: var(--padding-xs);
    }

    @keyframes shift
    {
      0%
      {
        opacity: 1;
      }

      50%
      {
        opacity: 0.3;
      }

      100%
      {
        opacity: 1;
      }
    }
  }
</style>
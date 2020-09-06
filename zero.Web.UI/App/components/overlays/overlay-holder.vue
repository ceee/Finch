<template>
  <div class="app-overlays" :class="{ 'has-multiple': instances.length > 1 }">
    <transition-group name="overlay" :duration="600">
      <div class="app-overlay-outer" :display="instance.display" v-for="(instance, index) in instances" :key="instance.id" :style="{ transform: 'translateX(' + (editorLength - index - 1) * -60 + 'px)' }">
        <div class="app-overlay-bg" @click="close(instance)"></div>
        <dialog open class="app-overlay" :style="{ width: instance.width + 'px' }" :class="'theme-' + instance.theme" :display="instance.display">
          <component :is="instance.component" :model.sync="instance.model" :config="instance"></component>
        </dialog>
      </div>
    </transition-group>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay.js'
  import { filter as _filter } from 'underscore';

  export default {
    data: () => ({
      instances: Overlay.instances
    }),

    computed: {
      editorLength()
      {
        return this.instances.filter(x => x.display === 'editor').length;
      }
    },

    methods: {
      close(instance)
      {
        if (instance.softdismiss !== false)
        {
          instance.close();
        }
      }
    }
  }
</script>


<style lang="scss">
  .app-overlays
  {
    grid-column: span 2 / auto;
  }

  .app-overlay-outer
  {
    display: flex;
    position: fixed;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    z-index: 5;
    justify-content: center;
    align-items: center;
    transition: transform 0.4s ease-out;

    & + .app-overlay-outer .app-overlay
    {
      //top: -10px;
    }
  }

  .app-overlay-bg
  {
    position: absolute;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    background: var(--color-overlay-shade);
    z-index: 2;
    opacity: 1;
  }

  .app-overlay
  {
    width: auto;
    height: auto;
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    border: none !important;
    box-shadow: 0 0 20px var(--color-shadow);
    padding: var(--padding);
   // max-width: 460px;
    text-align: center;
    position: relative;
    -webkit-backface-visibility: hidden;
    z-index: 3;
    color: var(--color-fg);
    font-size: var(--font-size);

    &.theme-dark
    {
      box-shadow: none;
    }
  }

  .theme-dark .app-overlay.theme-dark
  {
    box-shadow: 0 0 20px var(--color-shadow);
  }

  .app-overlay[display="dialog"] .ui-form-loading
  {
    height: 200px;
  }

  .app-overlay[display="editor"]
  {
    width: auto;
    position: absolute;
    left: auto;
    right: 0;
    top: 0;
    bottom: 0;
    border-radius: 0;
    box-shadow: -30px 0 40px var(--color-shadow);
    text-align: left;
    padding: 0;
    max-width: 100%;
  }

  .overlay-enter-active, .overlay-leave-active
  {
    .app-overlay-bg
    {
      transition: opacity .3s ease;
    }

    .app-overlay
    {
      transition: transform .3s ease, opacity .3s ease;
    }

    .app-overlay[display="editor"]
    {
      transition: transform .6s ease;
    }
  }

  .overlay-enter, .overlay-leave-to
  { 
    .app-overlay-bg
    {
      opacity: 0;
    }

    .app-overlay
    {
      opacity: 0;
      transform: scale(0.95);
    }

    .app-overlay[display="editor"]
    {
      opacity: 1;
      transform: scale(1) translateX(100%);
    }
  }

</style>
<template>
  <div class="app-overlays" :class="{ 'has-multiple': instances.length > 1 }">
    <transition-group name="overlay" :duration="300">
      <div class="app-overlay-outer" v-for="instance in instances" :key="instance.id">
        <div class="app-overlay-bg" @click="close"></div>
        <dialog open class="app-overlay">
          <component :is="instance.component" :model.sync="instance.model" :overlay="instance"></component>
        </dialog>
      </div>
    </transition-group>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay.js'

  export default {
    data: () => ({
      instances: Overlay.instances
    }),

    methods: {
      close()
      {
        Overlay.close();
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
    will-change: opacity;

    & + .app-overlay-outer .app-overlay
    {
      top: -10px;
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
    will-change: opacity;
    opacity: 1;
  }

  .app-overlay
  {
    width: auto;
    height: auto;
    background: var(--color-box);
    border-radius: var(--radius);
    border: none !important;
    box-shadow: 0 0 20px var(--color-shadow);
    padding: var(--padding);
    max-width: 460px;
    text-align: center;
    position: relative;
    will-change: transform, opacity;
    -webkit-backface-visibility: hidden;
    z-index: 3;
    color: var(--color-fg);
    font-size: var(--font-size);
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
  }

</style>
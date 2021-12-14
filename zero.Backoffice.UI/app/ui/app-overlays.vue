<template>
  <div class="app-overlays" :class="{ 'has-multiple': instances.length > 1 }">
    <transition-group name="overlay" :duration="600">
      <div class="app-overlay-outer" :display="instance.display" v-for="(instance, index) in instances" :key="instance.id" 
           :style="{ transform: instance.display !== 'editor' ? null : 'translateX(' + (editorLength - index - 1) * -120 + 'px)' }"
           :class="instance.class || ''">
        <div class="app-overlay-bg" @click="remove(instance)"></div>
        <div open class="app-overlay" :data-alias="instance.alias" :style="{ width: instance.width ? (instance.width + 'px') : null }" :class="'theme-' + instance.theme" :display="instance.display">
          <component :is="instance.component" :model.sync="instance.model" :config="instance" v-bind="instance" title=""></component>
        </div>
      </div>
    </transition-group>
  </div>
</template>


<script lang="ts">
  import emitter from '../services/eventhub';
  import { event_showOverlay, event_closeOverlays, event_finalizeOverlay, event_closeOverlay } from '../services/overlay';
  import { arrayRemove } from '../utils/arrays';
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'app-overlays',

    data: () => ({
      instances: []
    }),

    computed: {
      editorLength()
      {
        return this.instances.filter(x => x.display === 'editor').length;
      }
    },

    created()
    {
      emitter.on(event_showOverlay, overlay => this.add(overlay));
      emitter.on(event_closeOverlays, () => this.instances = []);
      emitter.on(event_finalizeOverlay, opts => this.finalize(opts.eventType, opts.instance, opts.value, opts.force));
      emitter.on(event_closeOverlay, overlay => this.remove(overlay, true));
    },

    mounted()
    {
      this.$el.addEventListener('keyup', e =>
      {
        if (e.key === "Escape" && this.instances.length) 
        {
          this.instances[this.instances.length - 1].close();
        }
      });
    },

    beforeDestroy()
    {
      this.$el.removeEventListener('keyup');
    },

    methods: {

      add(instance)
      {
        this.instances.push(instance);
      },

      remove(instance, force)
      {
        if (instance.softdismiss !== false || force)
        {
          arrayRemove(this.instances, instance);
        }
      },

      finalize(eventType, instance, value, force)
      {
        if ((eventType === 'close' && force) || (eventType === 'confirm' && instance.autoclose))
        {
          this.remove(instance);
        }
      }
    }
  })
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
    align-items: flex-start;
    transition: transform 0.55s ease-out;

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
    background: var(--color-overlay);
    border-bottom-right-radius: var(--radius);
    border-bottom-left-radius: var(--radius);
    border: none !important;
    box-shadow: var(--shadow-overlay-dialog);
    padding: var(--padding);
    padding-top: 40px;
    text-align: left;
    position: relative;
    -webkit-backface-visibility: hidden;
    z-index: 3;
    color: var(--color-text);
    font-size: var(--font-size);
  }

  .app-overlay .ui-loading
  {
    position: relative;
    left: 50%;
    margin-left: -16px;
    margin-top: 20px;
    margin-bottom: 20px;
  }

  .app-overlay .ui-headline
  {
    margin-bottom: 0.4em;
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
    box-shadow: var(--shadow-overlay);
    background: var(--color-overlay-editor);
    text-align: left;
    padding: 0;
    height: 100vh;
    max-width: 100%;
    border-bottom-right-radius: 0;
    border-top-left-radius: var(--radius);
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
      transform: translateY(-20px);
    }

    .app-overlay[display="editor"]
    {
      opacity: 1;
      transform: scale(1) translateX(100%);
    }
  }
</style>
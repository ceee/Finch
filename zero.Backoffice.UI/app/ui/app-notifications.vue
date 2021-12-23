<template>
  <transition-group tag="div" name="app-notifications" :duration="4000" class="app-notifications" :class="{ 'has-multiple': instances.length > 1 }">
    <div class="app-notification" v-for="instance in instances" :key="instance.id" :type="instance.type">
      <p class="-text">
        <b v-localize="instance.label"></b>
        <span v-if="instance.text" v-localize="instance.text"></span>
      </p>
      <ui-icon-button @click="remove(instance)" type="action small" icon="fth-x" title="@ui.close" />
    </div>
  </transition-group>
</template>


<script>
  import emitter from '../services/eventhub';
  import { event_showNotification, event_hideNotifications } from '../services/notification';
  import { arrayRemove } from '../utils/arrays';

  export default {
    name: 'app-notifications',

    data: () => ({
      instances: []
    }),

    created()
    {
      emitter.on(event_showNotification, notification => this.add(notification));
      emitter.on(event_hideNotifications, () => this.instances = []);
    },

    methods: {

      add(instance)
      {
        this.instances.push(instance);

        if (!instance.persistent)
        {
          setTimeout(() => this.remove(instance), instance.duration);
        }
      },

      remove(instance)
      {
        arrayRemove(this.instances, instance);
      }
    }
  }
</script>


<style lang="scss">
  .app-notifications
  {
    position: fixed;
    right: var(--padding);
    bottom: var(--padding);
    width: 420px;
    max-width: 100%;
    z-index: 6;
  }

  .app-notification
  {
    display: grid;
    grid-template-columns: 1fr auto;
    gap: 20px;
    align-items: center;
    background: var(--color-primary);
    color: var(--color-primary-text);
    border-radius: var(--radius);
    padding: 10px 12px;
    font-size: var(--font-size);
    transition: transform .3s ease, opacity .3s ease;

    .-text
    {
      margin: 0;

      span
      {
        font-size: var(--font-size-s);
        line-height: 1.3;
      }

      b
      {
        display: block;
        margin: 3px 0;
      }
    }

    .ui-icon-button
    {
      background: none;
      margin-right: -5px;
    }

    .ui-button-icon
    {
      color: var(--color-primary-text);
    }

    & + .app-notification
    {
      margin-top: 10px;
    }

    &[type="success"], &[type="primary"]
    {
      background: var(--color-primary);
      color: var(--color-primary-text);

      .ui-button-icon
      {
        color: var(--color-primary-text);
      }
    }

    &[type="error"]
    {
      background: var(--color-accent-error);
      color: white;

      .ui-button-icon
      {
        color: white;
      }
    }
  }

  .app-notification.app-notifications-enter,
  .app-notification.app-notifications-leave-to
  {
    opacity: 0;
    transform: translateY(20px);
  }
</style>
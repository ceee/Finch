<template>
  <div class="app-notifications" :class="{ 'has-multiple': instances.length > 1 }">
    <transition-group name="app-notifications" :duration="400">
      <div class="app-notification" v-for="instance in instances" :key="instance.id" :type="instance.type">
        <p class="-text">
          <b v-localize="instance.label"></b>
          <span v-if="instance.text" v-localize="instance.text"></span>
        </p>
        <ui-icon-button @click="close(instance)" type="action small" icon="fth-x" title="@ui.close" />
      </div>
    </transition-group>
  </div>
</template>


<script>
  import Notification from 'zero/services/notification.js'

  export default {
    data: () => ({
      instances: Notification.instances
    }),

    methods: {
      close(instance)
      {
        instance.close();
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
  }

  .app-notification
  {
    display: grid;
    grid-template-columns: 1fr auto;
    grid-gap: 20px;
    align-items: center;
    background: var(--color-primary);
    color: var(--color-primary-fg);
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
      color: var(--color-primary-fg);
    }

    & + .app-notification
    {
      margin-top: 10px;
    }

    &[type="success"], &[type="primary"]
    {
      background: var(--color-primary);
      color: var(--color-primary-fg);

      .ui-button-icon
      {
        color: var(--color-primary-fg);
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
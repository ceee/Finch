<template>
  <p class="ui-message" :class="messageClasses">
    <i v-if="iconClass" class="ui-message-icon" :class="iconClass"></i>
    <span class="ui-message-text" v-localize:html="text"></span>
  </p>
</template>


<script>
  const TYPE_ICONS = {
    neutral: 'fth-info',
    info: 'fth-info',
    warn: 'fth-alert-circle',
    error: 'fth-alert-circle',
    success: 'fth-check-circle'
  };

  export default {
    name: 'uiMessage',

    props: {
      type: {
        type: String,
        default: 'info'
      },
      text: {
        type: String,
        required: true
      },
      icon: {
        type: String,
        default: '-1'
      },
      block: {
        type: Boolean,
        default: true
      }
    },

    computed: {
      iconClass()
      {
        return this.icon !== '-1' ? this.icon : TYPE_ICONS[this.type];
      },
      messageClasses()
      {
        return [
          'type-' + this.type,
          (this.block ? 'block' : null)
        ];
      }
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>

<style lang="scss">
  .ui-message
  {
    font-size: var(--font-size-s);
    background: var(--color-accent-info-bg);
    color: var(--color-accent-info);
    display: inline-grid;
    padding: 8px 12px 7px 12px;
    grid-template-columns: auto 1fr;
    grid-gap: 12px;
    border-radius: var(--radius);
    position: relative;
    line-height: 20px;
    text-align: left;

    &.type-warn
    {
      background: var(--color-accent-warn-bg);
      color: var(--color-accent-warn);
    }

    &.type-error
    {
      background: var(--color-accent-error-bg);
      color: var(--color-accent-error);
    }

    &.type-success
    {
      background: var(--color-accent-success-bg);
      color: var(--color-accent-success);
    }

    &.type-neutral
    {
      background: var(--color-bg);
      color: var(--color-fg);
    }

    &.block
    {
      display: grid;
    }
  }

  .ui-message-icon
  {
    font-size: 1.3em;
    position: relative;
    top: 1px;
  }

  .ui-message-text
  {

  }
</style>
<template>
  <p class="ui-message" :class="messageClasses">
    <i v-if="iconClass" class="ui-message-icon" :class="iconClass"></i>
    <span class="ui-message-text" v-localize:html="text"></span>
  </p>
</template>


<script>
  export default {
    name: 'uiProperty',

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
        return this.icon !== '-1' ? this.icon : (this.type === 'info' ? 'fth-info' : 'fth-alert-circle');
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
    font-size: var(--font-size-m);
    background: var(--color-accent-info-bg);
    color: var(--color-accent-info);
    display: inline-grid;
    padding: 8px 12px;
    grid-template-columns: auto 1fr;
    grid-gap: 10px;
    border-radius: var(--radius);
    position: relative;
    line-height: 20px;

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

    &.block
    {
      display: grid;
    }
  }

  .ui-message-icon
  {
    font-size: 1.2em;
    position: relative;
    top: 1px;
  }

  .ui-message-text
  {

  }
</style>
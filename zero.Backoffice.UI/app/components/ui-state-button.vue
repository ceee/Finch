<template>
  <div class="ui-state-button" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <button v-for="item in items" @click="select(item.value)" v-localize="item.label"
            type="button" class="ui-state-button-item" :disabled="disabled || item.disabled" :class="{ 'is-active': value === item.value }"></button>
  </div>
</template>


<script>
  export default {
    props: {
      disabled: {
        type: Boolean,
        default: false
      },
      items: {
        type: Array,
        default: []
      },
      value: {
        type: [ String, Number ],
        default: null
      }
    },

    methods: {

      select(value)
      {
        if (!this.disabled)
        {
          this.$emit('input', value);
        }
      }

    }

  }
</script>

<style>
  .ui-state-button
  {
    display: inline-flex;
    gap: 3px;
  }

  .ui-state-button.is-disabled button
  {
    cursor: default;
  }

  button.ui-state-button-item
  {
    background: var(--color-input);
    padding: 10px 16px;
    color: var(--color-text-dim);
  }

  button.ui-state-button-item + .ui-state-button-item
  {
    border-left: none;
  }

  button.ui-state-button-item:first-of-type
  {
    border-top-left-radius: var(--radius);
    border-bottom-left-radius: var(--radius);
  }

  button.ui-state-button-item:last-child
  {
    border-top-right-radius: var(--radius);
    border-bottom-right-radius: var(--radius);
  }

  button.ui-state-button-item.is-active
  {
    color: var(--color-text);
    font-weight: bold;
  }
</style>
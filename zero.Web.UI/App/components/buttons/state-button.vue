<template>
  <div class="ui-state-button" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <button v-for="item in items" @click="select(item.value)" v-localize="item.label"
            type="button" class="ui-state-button-item" :disabled="disabled || item.disabled" :class="{ 'is-active': value === item.value }"></button>
  </div>
</template>


<script>
  import { find as _find } from 'underscore';

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

<style lang="scss">
  .ui-state-button
  {
    display: inline-flex;
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

    & + .ui-state-button-item
    {
      border-left: none;
    }

    &:first-of-type
    {
      border-top-left-radius: var(--radius);
      border-bottom-left-radius: var(--radius);
    }

    &:last-child
    {
      border-top-right-radius: var(--radius);
      border-bottom-right-radius: var(--radius);
    }

    &.is-active
    {
      //background: var(--color-bg-bright-three);
      color: var(--color-text);
      font-weight: bold;
    }
   
  }
</style>
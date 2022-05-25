<template>
  <div v-if="items" class="ui-radio-button" :class="{'is-disabled': disabled, 'is-horizontal': horizontal }" :style="{ 'grid-template-columns': 'repeat(' + items.length + ', minmax(0, 1fr))' }">
    <label v-for="item in items" class="ui-radio-button-item" :class="{ 'is-active': value === item.value }">
      <input type="radio" :value="item.value" :checked="item.value === value" @input="onChange(item.value)" :disabled="disabled || item.disabled" />
      <div class="-output">
        <span class="-check"><ui-icon symbol="fth-check" :size="12" :stroke="3"></ui-icon></span>
        <div>
          <span class="-title">
            <ui-icon class="-icon" v-if="item.icon" :symbol="item.icon" :size="14"></ui-icon>
            <span v-localize="item.label"></span>
          </span>
          <span class="-description" v-if="item.description" v-localize="item.description"></span>
        </div>
      </div>
    </label>
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
      },
      horizontal: {
        type: Boolean,
        default: true
      }
    },

    methods: {

      onChange(value)
      {
        if (!this.disabled)
        {
          this.$emit('update:value', value);
          this.$emit('input', value);
        }
      }

    }

  }
</script>

<style lang="scss">
  .ui-radio-button
  {
    //display: flex;
    &.is-horizontal
    {
      display: grid;
    }
  }

  .ui-radio-button-item
  {
    display: flex;
    align-items: center;
    width: 100%;
    position: relative;
    padding: var(--padding-s);
    border: 1px solid var(--color-line);
    background: var(--color-box);
    cursor: pointer;

    &.is-active
    {
      //border-color: var(--color-accent);
      //box-shadow: inset 0 0 0 1px var(--color-accent);
      //border-radius: var(--radius);
      background: var(--color-box-nested);
      z-index: 2;
      cursor: default;
    }

    .-output
    {
      display: grid;
      align-items: center;
      grid-template-columns: auto minmax(0, 1fr);
      grid-gap: var(--padding-s);
      line-height: 1.5;
    }

    .-title
    {
      font-weight: 700;
      display: block;
    }

    .-description
    {
      display: block;
      font-size: var(--font-size-s);
      color: var(--color-text-dim);
    }

    .-icon
    {
      display: none;
    }

    .-check
    {
      display: inline-flex;
      justify-content: center;
      align-items: center;
      width: 22px;
      height: 22px;
      border-radius: 20px;
      background: var(--color-bg-shade-4);

      .ui-icon
      {
        display: none;
      }
    }

    &.is-active .-check
    {
      background: var(--color-accent);
      color: var(--color-accent-fg);

      .ui-icon
      {
        display: inline;
      }
    }

    &:first-child
    {
      border-top-left-radius: var(--radius);
      border-top-right-radius: var(--radius);
    }

    &:last-child
    {
      border-bottom-left-radius: var(--radius);
      border-bottom-right-radius: var(--radius);
    }

    .is-horizontal &
    {
      & + .ui-radio-button-item
      {
        margin-top: 0;
        margin-left: -1px;
      }

      &:first-child
      {
        border-top-right-radius: 0;
        border-bottom-left-radius: var(--radius);
      }

      &:last-child
      {
        border-bottom-left-radius: 0;
        border-top-right-radius: var(--radius);
      }
    }

    & + .ui-radio-button-item
    {
      margin-top: -1px;
    }

    input
    {
      position: absolute;
      opacity: 0;
    }
  }

</style>
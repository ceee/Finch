<template>
  <svg class="ui-icon" :width="size" :height="size" :stroke-width="stroke" :data-symbol="symbolName" :class="classes">
    <use v-show="!isFlag" v-bind="{ 'xlink:href': href }" />
  </svg>
</template>


<script lang="ts">
  export default {
    props: {
      symbol: {
        type: String,
        default: null,
        required: true
      },
      file: {
        type: String,
        default: null
      },
      size: {
        type: Number,
        default: 17
      },
      stroke: {
        type: Number,
        default: 2
      }
    },


    computed: {

      symbolName()
      {
        return this.symbol && this.symbol.split(' ')[0];
      },
      classes()
      {
        return this.symbol ? this.symbol.split(' ').slice(1) : [];
      },
      isFlag()
      {
        return this.symbol && this.symbol.indexOf('flag') === 0;
      },
      href()
      {
        return (this.file || '') + '#' + this.symbolName.trim();
      }
    }
  }
</script>

<style lang="scss">
  .ui-icon
  {
    stroke: currentColor;
    stroke-linecap: round;
    stroke-linejoin: round;
    //fill: var(--color-bg-shade-3);
    fill: none;
  }

  .ui-icon[data-symbol="fth-waffle"]
  {
    stroke: none;
    fill: currentColor;
  }
</style>
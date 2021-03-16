<template>
  <svg class="ui-icon" :width="size" :height="size" :stroke-width="stroke" :data-symbol="symbol">
    <use v-show="!isFlag" v-bind="{ 'xlink:href': href }" />
  </svg>
</template>


<script>
  export default {
    name: 'uiIcon',

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
      isFlag()
      {
        return this.symbol && this.symbol.indexOf('flag') === 0;
      },
      href()
      {
        return (this.file || '') + '#' + this.symbol;
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
    fill: none;
  }

  .ui-icon[data-symbol="fth-waffle"]
  {
    stroke: none;
    fill: currentColor;
  }
</style>
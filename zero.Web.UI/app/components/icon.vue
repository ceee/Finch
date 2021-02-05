<template>
  <svg class="ui-icon" :width="size" :height="size" :stroke-width="stroke" :data-symbol="symbol">
    <use v-if="!isFlag" :xlink:href="'#' + symbol.trim()" />
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
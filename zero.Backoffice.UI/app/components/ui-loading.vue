<template>
  <i class="ui-loading" :class="{'is-big': isBig || big, 'is-small': small }"></i>
</template>


<script lang="ts">
  export default {
    props: {
      isBig: {
        type: Boolean,
        default: false
      },
      big: {
        type: Boolean,
        default: false
      },
      small: {
        type: Boolean,
        default: false
      }
    }
  }
</script>

<style lang="scss">
  .ui-loading
  {
    display: inline-block;
    width: 28px;
    height: 28px;
    border-radius: 40px;
    //border: 2px solid var(--color-box);
    //border-left-color: var(--color-text);
    border: 2px solid transparent;
    border-left-color: var(--color-text);
    will-change: transform;
    animation: loadingRotation 0.8s linear infinite;

    &.is-big
    {
      width: 36px;
      height: 36px;
      border-width: 2px;
    }

    &.is-small
    {
      width: 18px;
      height: 18px;
    }
  }

  @keyframes loadingRotation
  {
    0%
    {
      transform: rotate(0)
    }
    100%
    {
      transform: rotate(1turn)
    }
  }
</style>
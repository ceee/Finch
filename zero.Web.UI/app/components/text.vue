<template>
  <div class="ui-text" @click="opened = !opened" :class="{ 'is-open': opened }" :style="{'-webkit-line-clamp': maxLines > 0 ? maxLines : null }"><slot></slot></div>
</template>

<script>
  export default {
    name: 'uiText',

    props: {
      maxLines: {
        type: Number
      }
    },

    data: () => ({
      opened: false
    })
  }
</script>

<style lang="scss">
  .ui-text
  {
    line-height: 1.5;
    display: -webkit-box;
    -webkit-box-orient: vertical;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: normal;

    &.is-open
    {
      -webkit-line-clamp: inherit !important;
    }
  }
</style>
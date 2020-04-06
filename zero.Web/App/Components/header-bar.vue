<template>
  <div class="ui-header-bar">
    <h2 v-if="title" class="ui-header-bar-title" v-localize="title"></h2>
    <slot></slot>
  </div>
</template>


<script>
  export default {
    name: 'uiHeaderBar',

    props: {
      title: {
        type: String
      },
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }
</script>

<style lang="scss">
  @import 'Sass/Core/all';

  .ui-header-bar
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    height: 80px;
    padding: 0 var(--padding);
  }

  .ui-header-bar-title
  {
    @extend %font;
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 700;
  }
</style>
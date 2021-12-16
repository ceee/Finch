<template>
  <div>
    selection
  </div>
</template>

<script>
  export default {
    name: 'mediaOverviewSelection',

    props: {
      value: {
        type: Object,
        default: () => { }
      },
      selected: {
        type: Boolean,
        default: false
      }
    },
  };
</script>

<style lang="scss">
  
</style>

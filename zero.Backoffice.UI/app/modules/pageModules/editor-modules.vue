<template>
  <ui-modules :value="[]" />
</template>


<script>
  export default {
    props: {
      value: Object,
      config: Object,
      limit: {
        type: Number,
        default: 1
      }
    }
  }
</script>
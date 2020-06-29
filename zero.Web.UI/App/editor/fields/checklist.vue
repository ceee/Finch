<template>
  <ui-check-list :value="value" @input="$emit('input', $event)" :reverse="config.reverse" :items="config.items" />
</template>

<script>
  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      },
      config: Object
    },
  }
</script>
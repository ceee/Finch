<template>
  <ui-pagepicker :value="value" @input="$emit('input', $event)" :model="config.model" :limit="limit" :disabled="config.disabled" :root-id="rootId" :disabled-ids="disabledIds" />
</template>


<script>
  export default {
    props: {
      value: [String, Array],
      config: Object,
      limit: {
        type: Number,
        default: 1
      },
      rootId: [String, Function],
      disabledIds: {
        type: [Array, Function],
        default: []
      }
    }
  }
</script>
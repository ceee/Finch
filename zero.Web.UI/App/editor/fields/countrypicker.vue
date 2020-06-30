<template>
  <ui-countrypicker :value="value" @input="$emit('input', $event)" :limit="config.limit || 1" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Array]
      },
      config: Object
    }
  }
</script>
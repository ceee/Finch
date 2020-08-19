<template>
  <ui-modules :value="value" @input="$emit('input', $event)" :config="config" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [String, Object, Array]
      },
      config: Object
    }
  }
</script>
<template>
  <ui-state-button :items="items" :value="value" @input="$emit('input', $event)" :disabled="config.disabled" />
</template>


<script>
  export default {
    props: {
      value: [ String, Number ],
      config: Object,
      items: [Array, Function]
    }
  }
</script>
<template>
  <textarea class="ui-textarea" :value="value" @input="$emit('input', $event.target.value)" rows="3" :disabled="config.disabled"></textarea>
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object
    },
  }
</script>
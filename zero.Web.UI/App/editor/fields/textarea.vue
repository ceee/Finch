<template>
  <textarea class="ui-textarea" :value="value" @input="$emit('input', $event.target.value)" rows="3"></textarea>
</template>


<script>
  export default {
    props: {
      value: {
        type: String
      },
      config: Object
    },
  }
</script>
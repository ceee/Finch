<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-localize:placeholder="placeholder" :maxlength="maxLength" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      maxLength: {
        type: Number,
        default: null
      },
      placeholder: {
        type: String,
        default: null
      }
    }
  }
</script>
<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="password" class="ui-input" v-localize:placeholder="placeholder" :maxlength="maxLength" :disabled="disabled" />
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
      disabled: {
        type: Boolean,
        default: false
      },
      placeholder: {
        type: String,
        default: null
      }
    }
  }
</script>
<template>
  <div class="ui-box">
    <input :value="value" @input="$emit('input', +$event.target.value)" type="text" class="ui-input" v-localize:placeholder="placeholder" :disabled="disabled" />
  </div>
</template>


<script>
  export default {
    props: {
      value: {
        type: Number,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      placeholder: String
    },
  }
</script>
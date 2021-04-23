<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder, model: entity }" :maxlength="maxLength" :disabled="disabled" />
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
        type: [String, Function],
        default: null
      },
      entity: Object
    }
  }
</script>
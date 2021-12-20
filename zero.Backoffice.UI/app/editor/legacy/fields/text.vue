<template>
  <input :model-value="modelValue" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder, model: entity }" :maxlength="maxLength" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      modelValue: {
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
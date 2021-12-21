<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder, model: entity }" :maxlength="maxLength" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: String,
      entity: Object,
      disabled: Boolean,

      maxLength: {
        type: Number,
        default: null
      },
      placeholder: {
        type: [String, Function],
        default: null
      }
    }
  }
</script>
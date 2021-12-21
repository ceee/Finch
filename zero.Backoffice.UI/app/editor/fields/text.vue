<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder, model: config.model }" :maxlength="maxLength" :disabled="config.disabled" />
</template>


<script>
  export default {
    props: {
      value: String,
      config: Object,

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
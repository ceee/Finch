<template>
  <input :value="value" @input="onChange($event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder, model: config.model }" :maxlength="maxLength" :disabled="config.disabled" />
</template>


<script>
  export default {
    props: {
      value: Number,
      config: Object,

      maxLength: {
        type: Number,
        default: null
      },
      placeholder: {
        type: [String, Function],
        default: null
      }
    },

    methods: {
      onChange(value)
      {
        var parsedValue = parseFloat(value);

        if (isNaN(parsedValue))
        {
          return;
        }

        this.$emit('input', parsedValue);
      }
    }
  }
</script>
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
        let parsedValue = null;
        let optional = this.config.field.optional(this.config.model);

        if (value !== null && value.trim() === '')
        {
          parsedValue = optional ? null : 0;
        }
        else
        {
          parsedValue = parseFloat(value);

          if (isNaN(parsedValue))
          {
            return;
          }
        }

        console.info('val', parsedValue);

        this.$emit('input', parsedValue);
      }
    }
  }
</script>
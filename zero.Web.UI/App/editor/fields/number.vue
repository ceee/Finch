<template>
  <input :value="value" @input="onChange($event.target.value)" type="text" class="ui-input" v-localize:placeholder="" :maxlength="maxLength" :disabled="disabled" />
</template>


<script>
  export default {
    emits: ['input'],

    props: {
      value: {
        type: Number,
        default: null
      },
      config: Object,
      disabled: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      maxLength()
      {
        return this.config.maxLength > 0 ? this.config.maxLength : null;
      },
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
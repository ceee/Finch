<template>
  <input :value="value" @input="onChange($event.target.value)" type="text" class="ui-input" v-localize:placeholder="" :maxlength="maxLength" />
</template>


<script>
  export default {
    props: {
      value: {
        type: Number,
        default: null
      },
      config: Object
    },

    computed: {
      maxLength()
      {
        return this.config.options.maxLength > 0 ? this.config.options.maxLength : null;
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
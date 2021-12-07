<template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="modelValue" @input="onChange" :disabled="disabled">
      <option v-if="emptyOption"></option>
      <option v-for="option in options" :value="option.key" v-localize="option.value"></option>
    </select>
  </div>
</template>


<script>
  export default {
    name: 'uiSelect',

    props: {
      modelValue: [String, Number, Object],
      items: [Array, Function],
      entity: Object,
      disabled: Boolean,
      emptyOption: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      options: []
    }),

    created()
    {
      this.rebuild();
    },

    watch: {
      items: {
        deep: true,
        handler()
        {
          this.rebuild();
          this.onChange({ target: { value: this.modelValue } });
        }
      },
      entity: {
        deep: true,
        handler()
        {
          this.rebuild();
          this.onChange({ target: { value: this.modelValue } });
        }
      }
    },

    methods: {
      rebuild()
      {
        if (this.entity && typeof this.items === 'function')
        {
          this.options = this.items(this.entity);
        }
        else if (typeof this.items !== 'function' && this.items)
        {
          this.options = [...this.items];
        }
        else
        {
          this.options = [];
        }
      },

      onChange(ev)
      {
        this.$emit('update:modelValue', ev.target.value ? ev.target.value : null);
      }
    }
  }
</script>
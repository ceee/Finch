<template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="onChange" :disabled="disabled">
      <option v-if="emptyOption"></option>
      <option v-for="option in options" :value="option.key" v-localize="option.value"></option>
    </select>
  </div>
</template>


<script>
  export default {
    props: {
      value: [String, Number, Object],
      items: [Array, Function],
      entity: {
        type: Object,
        required: true
      },
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
          this.onChange({ target: { value: this.value } });
        }
      },
      entity: {
        deep: true,
        handler()
        {
          this.rebuild();
          this.onChange({ target: { value: this.value } });
        }
      }
    },

    methods: {
      rebuild()
      {
        let items = [];

        if (!this.entity || !this.items)
        {
          this.options = items;
          return;
        }

        if (typeof this.items === 'function')
        {
          this.options = this.items(this.entity);
        }
        else
        {
          this.options = [...this.items];
        }
      },

      onChange(ev)
      {
        this.$emit('input', ev.target.value ? ev.target.value : null);
      }
    }
  }
</script>
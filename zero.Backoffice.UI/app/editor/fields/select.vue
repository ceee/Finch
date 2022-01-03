<template>
  <div class="ui-native-select" :disabled="config.disabled">
    <select :value="value" @input="onChange" :disabled="config.disabled">
      <option v-if="emptyOption"></option>
      <option v-for="option in options" :value="option.value" v-localize="option.label"></option>
    </select>
  </div>
</template>


<script>
  export default {
    props: {
      value: [String, Number],
      config: Object,
      items: [Array, Function],
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
      'config.model.addresses': {
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

        if (!this.config.model || !this.items)
        {
          this.options = items;
          return;
        }

        if (typeof this.items === 'function')
        {
          this.options = this.items(this.config.model);
        }
        else
        {
          this.options = [...this.items];
        }
      },

      onChange(ev)
      {
        let value = ev.target.value || null;

        if (value && !this.options.find(x => x.value == value))
        {
          value = null;
        }

        this.$emit('input', value);
      }
    }
  }
</script>
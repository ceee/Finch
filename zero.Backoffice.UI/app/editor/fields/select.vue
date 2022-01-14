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
        handler(val)
        {
          if (typeof val !== 'function')
          {
            this.rebuild();
          }
        }
      },
      'config.model': {
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
        let options = [];

        if (!this.config.model || !this.items)
        {
          options = items;
        }
        else if (typeof this.items === 'function')
        {
          options = this.items(this.config.model);
        }
        else
        {
          options = [...this.items];
        }

        if (JSON.stringify(options) !== JSON.stringify(this.options))
        {
          this.options = options;
        }
      },

      onChange(ev)
      {
        let value = ev.target.value || null;

        if (value && !this.options.find(x => x.value == value))
        {
          value = null;
        }

        if (this.value === value)
        {
          return;
        }

        this.$emit('input', value);
        this.$emit('update:value', value);
      }
    }
  }
</script>
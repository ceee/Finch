<template>
  <div class="ui-check-list" :class="{'is-disabled': disabled, 'is-inline': inline }">
    <label v-for="item in list" class="ui-native-check ui-check-list-item">
      <input type="checkbox" :checked="isChecked(item)" @input="onChange(item)" :disabled="disabled" />
      <span class="ui-native-check-toggle"></span>
      <span v-localize="item[labelKey]"></span>
      <span class="-desc" v-if="item[descriptionKey]" v-localize="item[descriptionKey]"></span>
    </label>
  </div>
</template>


<script>
  export default {
    name: 'uiCheckList',
    props: {
      value: {
        type: Array,
        default: () => []
      },
      items: {
        type: [Array, Function, Promise],
        required: true
      },
      disabled: {
        type: Boolean,
        default: false
      },
      inline: {
        type: Boolean,
        default: false
      },
      limit: {
        type: Number,
        default: 100
      },
      reverse: Boolean,
      labelKey: {
        type: String,
        default: 'value'
      },
      descriptionKey: {
        type: String,
        default: 'description'
      },
      idKey: {
        type: String,
        default: 'key'
      }
    },

    data: () => ({
      list: []
    }),

    watch: {
      items()
      {
        this.init();
      }
    },

    mounted()
    {
      this.init();
    },

    methods: {
      init()
      {
        if (typeof this.items === 'function')
        {
          this.items().then(res =>
          {
            this.list = res.data;
          });
        }
        else
        {
          this.list = JSON.parse(JSON.stringify(this.items));
        }
      },

      isChecked(item)
      {
        let index = this.value.indexOf(item[this.idKey]);
        return (!this.reverse && index > -1) || (this.reverse && index < 0);
      },

      onChange(item)
      {
        let index = this.value.indexOf(item[this.idKey]);
        let value = JSON.parse(JSON.stringify(this.value));
        if (index < 0)
        {
          value.push(item[this.idKey]);
        }
        else
        {
          value.splice(index, 1);
        }

        this.$emit('input', value);
        this.$emit('update:value', value);
      }
    }
  }
</script>

<style lang="scss">
  .ui-check-list-item
  {
    display: block;
  }

  .ui-check-list .ui-check-list-item + .ui-check-list-item
  {
    margin-top: 8px;
  }

  .ui-alias + .ui-check-list-item
  {
    margin-top: 14px;
  }

  .ui-check-list.is-inline .ui-check-list-item
  {
    display: inline-block;
  }

  .ui-check-list.is-inline .ui-check-list-item + .ui-check-list-item
  {
    margin-top: 0;
    margin-left: 30px;
  }

  .ui-check-list.is-inline .ui-check-list-item .ui-native-check-toggle
  {
    margin-right: 6px;
  }

  .ui-check-list-item .-desc
  {
    display: inline-block;
    color: var(--color-text-dim);
    font-size: var(--font-size-s);
    margin-left: 0.5em;

    &:before { content: '('; }
    &:after { content: ')'; }
  }
</style>
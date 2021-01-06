 <template>
  <div class="ui-input-list" :class="{'is-disabled': disabled }">
    <div v-for="item in items" class="ui-input-list-item">
      <input v-model="item.value" type="text" class="ui-input" :maxlength="maxLength" :readonly="disabled" @input="onChange" size="5" />
      <ui-icon-button type="light" icon="fth-x" @click="removeItem(item)" v-if="!disabled" />
    </div>
    <ui-button v-if="!disabled && !plusButton" type="light" :label="addLabel" @click="addItem" />
    <ui-select-button v-if="!disabled && plusButton" icon="fth-plus" :label="items.length > 0 ? null : addLabel" @click="addItem" />
  </div>
</template>


<script>
  import { map as _map } from 'underscore';

  export default {
    name: 'uiInputList',

    props: {
      addLabel: {
        type: String,
        default: '@ui.add'
      },
      value: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      },
      maxItems: {
        type: Number,
        default: 100
      },
      maxLength: {
        type: Number,
        default: 200
      },
      plusButton: {
        type: Boolean,
        default: false
      },
    },

    data: () => ({
      items: []
    }),

    watch: {
      value(value)
      {
        this.items = _map(value, item =>
        {
          return { value: item };
        });
      }
    },

    mounted()
    {
      this.items = _map(this.value, item =>
      {
        return { value: item };
      });
    },

    methods: {

      onChange()
      {
        this.$emit('input', _map(this.items, item => item.value));
      },

      addItem()
      {
        this.items.push({
          value: null
        });
        this.onChange();

        this.$nextTick(() =>
        {
          this.$el.querySelector('.ui-input-list-item:last-of-type input').focus();
        });
      },

      removeItem(item)
      {
        const index = this.items.indexOf(item);
        this.items.splice(index, 1);
        this.onChange();
      }
    }
  }
</script>

<style lang="scss">
  .ui-input-list
  {

  }

  .ui-input-list-item
  {
    display: grid;
    grid-template-columns: 1fr auto;
    background: var(--color-input);
    border-radius: var(--radius);

    & + .ui-input-list-item, & + .ui-button, & + .ui-select-button
    {
      margin-top: 6px;
    }

    .ui-input-list.is-disabled &
    {
      background: transparent;
    }

    .ui-input
    {
      border-radius: var(--radius) 0 0 var(--radius);
    }

    .ui-icon-button
    {
      border-radius: 0 var(--radius) var(--radius) 0;
      height: 48px;
      width: 48px;
      border-left: none;
      background: transparent !important;
      box-shadow: none;
    }
  }
</style>
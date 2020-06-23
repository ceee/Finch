 <template>
  <div class="ui-input-list" :class="{'is-disabled': disabled }">
    <div v-for="item in items" class="ui-input-list-item">
      <input v-model="item.value" type="text" class="ui-input" :maxlength="maxLength" :readonly="disabled" @input="onChange" />
      <ui-icon-button type="light" icon="fth-x" @click="removeItem(item)" v-if="!disabled" />
    </div>
    <ui-button v-if="!disabled" type="light" :label="addLabel" @click="addItem" />
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
      }
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

    & + .ui-input-list-item, & + .ui-button
    {
      margin-top: 6px;
    }

    .ui-input
    {
      border-radius: var(--radius) 0 0 var(--radius);
      border-right: none;
    }

    .ui-icon-button
    {
      border-radius: 0 var(--radius) var(--radius) 0;
      height: 42px;
      width: 42px;
      border: 1px solid var(--color-line);
      border-left: none;
      background: transparent !important;
    }
  }
</style>
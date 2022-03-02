<template>
  <div class="ui-input-list" :class="{'is-disabled': disabled }">
    <div class="ui-input-list-items" v-sortable="{ handle: '.is-handle', onUpdate: onSortingUpdated, enabled: !disabled }">
      <div v-for="item in items" :key="item.id" class="ui-input-list-item">
        <button type="button" class="ui-input-list-sort is-handle" tabindex="-1">
          <ui-icon symbol="fth-grip-vertical" />
        </button>
      
        <input v-model="item.value" type="text" class="ui-input" :maxlength="maxLength" :readonly="disabled" @change="onChange(items)" size="5" />
        <ui-icon-button type="light" icon="fth-x" @click="removeItem(item)" v-if="!disabled" tabindex="-1" />
      </div>
    </div>
    <ui-button v-if="!disabled && !plusButton" type="light" :label="addLabel" @click="addItem" />
    <ui-select-button v-if="!disabled && plusButton" icon="fth-plus" :label="items.length > 0 ? null : addLabel" @click="addItem" />
  </div>
</template>


<script>
  import { arrayMove, generateId } from '../utils';

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
        this.$nextTick(() =>
        {
          this.items = (value || []).map(item =>
          {
            return { id: generateId(), value: item };
          });
        });
      }
    },
    mounted()
    {
      this.items = (this.value || []).map(item =>
      {
        return { id: generateId(), value: item };
      });
    },
    methods: {
      onChange(items)
      {
        let value = items.map(item => item.value);
        this.$emit('input', value);
        this.$emit('update:value', value);
      },
      addItem()
      {
        this.items.push({
          id: generateId(),
          value: ''
        });
        this.onChange(this.items);
        this.$nextTick(() =>
        {
          this.$el.querySelector('.ui-input-list-item:last-of-type input').focus();
        });
      },
      removeItem(item)
      {
        const index = this.items.indexOf(item);
        this.items.splice(index, 1);
        this.onChange(this.items);
      },
      onSortingUpdated(ev)
      {
        let items = arrayMove(this.items, ev.oldIndex, ev.newIndex);
        this.onChange(items);
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
    grid-template-columns: auto 1fr auto;
    background: var(--color-input);
    border-radius: var(--radius);
    margin-bottom: 6px;

    .ui-input-list.is-disabled &
    {
      background: transparent;
    }

    .ui-icon-button
    {
      height: 48px;
      width: 48px;
      border-left: none;
      background: transparent !important;
      box-shadow: none;
    }

    .ui-icon-button + .ui-icon-button
    {
      margin-left: 0;
    }
  }

  button.ui-input-list-sort
  {
    height: 100%;
    width: 30px;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
    pointer-events: none;
    cursor: grab;
  }
</style>
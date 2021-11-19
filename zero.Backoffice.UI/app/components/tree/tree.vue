<template>
  <div class="ui-tree">
    <ui-header-bar class="ui-tree-header" :title="header" :back-button="false" v-if="header">
      <ui-dot-button @click="onActionsClicked(null, $event)" />
    </ui-header-bar>
    <slot></slot>
    <span v-if="status === 'loading'" class="ui-tree-item-loading"><i></i></span>
    <template v-for="item in items">
      <ui-tree-item :value="item" :active-id="active" :depth="depth" :selected="selection.indexOf(item.id) > -1"
                    @rightclick="onRightClicked" @click="onSelect(item, $event)" @actions="onActionsClicked" @open="toggle" @setactive="onActiveSet"
                    />
      <ui-tree v-if="item.hasChildren && item.isOpen && status != 'loading'" v-bind="{ get, parent: item.id, depth: depth + 1, active, mode, selection, selectionLimit }" @select="onChildSelect" @setactive="onActiveSet">
        <template v-slot:actions="props">
          <slot name="actions" v-bind="props"></slot>
        </template>
      </ui-tree>
    </template>
    <slot name="bottom"></slot>
    <ui-dropdown ref="dropdown" align="top" theme="dark" class="ui-tree-dropdown">
      <slot name="actions" v-bind="actionProps"></slot>
    </ui-dropdown>
  </div>
</template>


<script>
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {
    name: 'uiTree',

    props: {
      depth: {
        type: Number,
        default: 0
      },
      active: {
        type: String,
        default: null
      },
      parent: {
        type: String,
        default: null
      },
      header: {
        type: String,
        default: null
      },
      get: {
        type: Function,
        required: true
      },
      hasActions: {
        type: Function,
        default: null
      },
      mode: {
        type: String,
        default: 'link'
      },
      selection: {
        type: Array,
        default: () => []
      },
      selectionLimit: {
        type: Number,
        default: 1
      }
    },

    data: () => ({
      items: [],
      status: 'none',
      actionProps: {
        item: null
      }
    }),

    computed: {
      actionsDefined()
      {
        return this.$scopedSlots.hasOwnProperty('actions');
      }
    },

    mounted()
    {
      this.refresh();
    },

    methods: {

      onActiveSet(val)
      {
        this.$emit('setactive', val);
      },

      // refreshes the whole tree
      refresh()
      {
        this.load(this.parent);
      },


      // loads children of the given parent id or on root if empty
      load(parent)
      {
        this.setStatus('loading', this.items);

        let promise = this.get(parent, this.active);

        promise.then(response =>
        {
          this.items = response;
          this.setStatus('loaded', this.items);
        })
        .catch(error =>
        {
          console.error(error);
          this.setStatus('error', this.items, error);
          // TODO handle errors
        });
      },


      // updates the status for the current tree
      setStatus(status)
      {
        this.status = status;
        this.$emit('onStatusChange', status);
      },


      // toggles children of an item
      toggle(item)
      {
        item.isOpen = !item.isOpen;
      },


      // selected an item
      onSelect(item, ev)
      {
        if (this.mode === 'select')
        {
          let index = this.selection.indexOf(item.id);
          if (index > -1)
          {
            this.selection.splice(index, 1);
          }
          else if (this.selectionLimit === 1)
          {
            this.selection.splice(0, this.selection.length);
            this.selection.push(item.id);
          }
          else if (this.selection.length < this.selectionLimit)
          {
            this.selection.push(item.id);
          }

          this.$emit('select', this.selectionLimit > 1 ? this.selection : (this.selection.length > 0 ? this.selection[0] : null), ev);
        }
        else
        {
          this.$emit('select', item, ev);
        }
      },


      onChildSelect(item, ev)
      {
        this.$emit('select', item, ev);
      },


      // right clicked on an item
      onRightClicked(item, ev)
      {
        if (this.actionsDefined && (!item || item.hasActions))
        {
          ev.preventDefault();
          this.onActionsClicked(item, ev);
        }
      },


      // actions button clicked on item
      onActionsClicked(item, ev)
      {
        let dropdown = this.$refs.dropdown;

        if (!this.actionsDefined || (item && !item.hasActions) || (typeof this.hasActions === 'function' && !this.hasActions(item)))
        {
          return;
        }

        this.actionProps.item = item;
        this.actionProps.event = ev;

        dropdown.toggle();

        if (!dropdown.open)
        {
          return;
        }

        this.$nextTick(() =>
        {
          let target = ev.target;
          do
          {
            if (target.classList.contains('ui-tree-item') || target.classList.contains('ui-tree-header'))
            {
              break;
            }
          }
          while (target = target.parentElement);

          target = target.querySelector('.ui-dot-button');

          var rect = target.getBoundingClientRect();
          var width = 240;

          var position = {
            x: rect.left - width + rect.width,
            y: rect.top + rect.height
          };

          let element = dropdown.$el.querySelector('.ui-dropdown');

          element.style.top = position.y + 'px';
          element.style.left = position.x + 'px';
          element.style.width = width + 'px';
        });
      }

    }
  }
</script>


<style lang="scss">
  .ui-tree
  {
    position: relative;
    overflow-x: hidden;
  }

  .ui-tree-header + .ui-tree-item
  {
    margin-top: -18px;
  }

  .ui-tree-dropdown .ui-dropdown
  {
    position: fixed;
    min-width: 200px;
  }
</style>
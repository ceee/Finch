<template>
  <div class="ui-tree" :style="{ 'padding-left': depth > 0 ? (((depth > 0 ? 1 : 0) * 15) + 'px') : null }">
    <ui-header-bar class="ui-tree-header" :title="header" :back-button="false" v-if="header">
      <ui-dot-button @click="onActionsClicked(null, $event)" />
    </ui-header-bar>
    <slot></slot>
    <span v-if="status === 'loading'" class="ui-tree-item-loading"><i></i></span>
    <template v-for="item in items">
      <ui-tree-item :value="item" @rightclick="onRightClicked" @click="onSelect(item, $event)" @actions="onActionsClicked" @open="toggle" />
      <ui-tree v-if="item.hasChildren && item.isOpen && status != 'loading'" :get="get" :parent="item.id" :depth="depth + 1" :active="active" :config="config" @select="onSelect" />
    </template>
    <slot name="bottom"></slot>
    <div ref="dropdown" class="ui-dropdown ui-tree-dropdown theme-dark align-top" role="dialog" v-click-outside="hideActions" v-if="actionsOpen">
      <ui-dropdown-list v-model="actions" @hide="hideActions" />
    </div>
  </div>
</template>


<script>
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  const defaultConfig = {
    // return actions for an item
    onActionsRequested: null
  };

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
      config: {
        type: Object,
        default: () =>
        {
          return defaultConfig;
        }
      }
    },

    data: () => ({
      items: [],
      status: 'none',
      actionsOpen: false,
      actionsLoaded: false,
      actions: [],
      configuration: {}
    }),

    mounted()
    {
      this.initialize();
      this.load(this.parent);
    },

    methods: {

      initialize()
      {
        this.configuration = _extend(defaultConfig, this.config || {});
      },

      refresh()
      {
        this.initialize();
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

      // hide actions overlay
      hideActions()
      {
        if (this.actionsLoaded)
        {
          this.actionsOpen = false;
        }
      },

      // toggles children of an item
      toggle(item)
      {
        item.isOpen = !item.isOpen;
      },

      onSelect(item, ev)
      {
        this.$emit('select', item, ev);
      },

      // right clicked on an item
      onRightClicked(item, ev)
      {
        if (item.hasActions && this.configuration.onActionsRequested)
        {
          ev.preventDefault();  
          this.onActionsClicked(item, ev);
        }
      },

      // actions button clicked on item
      onActionsClicked(item, ev)
      {
        this.actionsLoaded = false;

        if (!this.actionsOpen)
        {
          this.actions = this.configuration.onActionsRequested(item);
        }

        if (!this.actions || this.actions.length < 1 || this.actionsOpen)
        {
          this.actionsLoaded = true;
          return;
        }

        this.actionsOpen = !this.actionsOpen;

        this.$nextTick(() =>
        {
          let dropdown = this.$refs.dropdown;
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

          dropdown.style.top = position.y + 'px';
          dropdown.style.left = position.x + 'px';
          dropdown.style.width = width + 'px';

          setTimeout(() =>
          {
            this.actionsLoaded = true;
          }, 300);
        });
      }

    }
  }
</script>


<style lang="scss">
  .ui-tree
  {
    position: relative;
  }

  .ui-tree-header + .ui-tree-item
  {
    margin-top: -18px;
  }

  .ui-tree-dropdown
  {
    position: fixed;
    min-width: 200px;
  }
</style>
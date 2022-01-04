<template>
  <div class="ui-tree-item" :class="getClasses(value)" v-on:contextmenu="onRightClicked(value, $event)">
    <button :disabled="value.disabled" v-if="value.hasChildren" @click="toggle(value)" type="button" class="ui-tree-item-toggle" :style="{ 'width': ((depth * 15) + 32) + 'px' }">
      <ui-icon class="ui-tree-item-arrow" :symbol="'fth-chevron-' + (value.isOpen ? 'up' : 'down')" :size="14" />
    </button>
    <span v-if="!value.hasChildren" class="ui-tree-item-toggle" :style="{ 'width': ((depth * 15) + 32) + 'px' }"></span>
    <component :disabled="value.disabled" :is="tag" type="button" :to="value.url" class="ui-tree-item-link" @click="onClick(value, $event)">
      <ui-icon class="ui-tree-item-icon" v-if="value.icon" :class="{'is-dashed': value.isDashed }" :symbol="value.icon" :size="18" />
      <ui-icon v-if="value.modifier" :title="value.modifier.name" class="ui-tree-item-modifier" :symbol="modifier" :class="modifierClass" :size="10" :stroke="2.5" />
      <span class="ui-tree-item-text">
        <ui-localize :value="value.name" />
        <span class="ui-tree-item-description" v-if="value.description">
        <br />
        <ui-localize :value="value.description" />
        </span>
      </span>
      
    </component>
    <ui-dot-button :disabled="value.disabled" class="ui-tree-item-actions" v-if="value.hasActions" @click="onActionsClicked(value, $event)" />
    <span class="ui-tree-item-count" v-if="value.countOutput != null">{{value.countOutput}}</span>
  </div>
</template>


<script lang="ts">
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'uiTreeItem',

    props: {
      value: {
        type: Object,
        required: true
      },
      activeId: {
        type: String,
        default: null
      },
      selected: {
        type: Boolean,
        default: false
      },
      depth: {
        type: Number,
        default: 0
      },
    },

    data: () => ({
      _isActive: false
    }),

    computed: {
      isLink()
      {
        return this.value && this.value.url;
      },
      tag()
      {
        return this.isLink ? 'router-link' : 'button';
      },
      modifier()
      {
        return this.value && this.value.modifier ? this.value.modifier.icon.split(' ')[0] : null;
      },
      modifierClass()
      {
        return this.value && this.value.modifier ? this.value.modifier.icon.split(' ')[1] : null;
      },
      isActive()
      {
        return this.value && this.isLink && (
          (!!this.value.id && this.value.id === this.activeId)
          || this.value.id == this.$route.params.id
          || (this.value.url && !this.value.url.params && this.value.url.name === this.$route.name && !this.$route.params.id)
        );
      }
    },

    watch: {
      isActive(val)
      {
        if (val)
        {
          this.$emit('setactive', {
            $el: this.$el,
            model: this.value
          });
        }
      }
    },

    mounted()
    {
      if (this.isActive)
      {
        this.$emit('setactive', {
          $el: this.$el,
          model: this.value
        });
      }
    },

    methods: {
      onClick(item, ev)
      {
        if (this.isLink)
        {
          return;
        }
        else
        {
          this.$emit('click', item, ev);
        }
      },

      // get all classes for a tree item
      getClasses(item)
      {
        return {
          'has-icon': !!item.icon,
          'has-children': item.hasChildren,
          'is-inactive': item.isInactive,
          'is-open': item.isOpen,
          'is-selected': this.selected || item.isSelected,
          'is-disabled': item.disabled,
          'is-active': this.isActive
        };
      },

      // toggles children of an item
      toggle(item)
      {
        this.$emit('open', item);
      },

      // right clicked on an item
      onRightClicked(item, ev)
      {
        if (!item.disabled)
        {
          this.$emit('rightclick', item, ev);
        }
      },

      // actions button clicked on item
      onActionsClicked(item, ev)
      {
        if (!item.disabled)
        {
          this.$emit('actions', item, ev);
        }
      }
    }
  })
</script>


<style lang="scss">

  .ui-tree-item
  {
    display: grid;
    grid-template-columns: auto 1fr auto auto;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 var(--padding) 0 0;
    //height: 54px;
    height: 58px;
    border-top: 1px dashed var(--color-line-dashed);
    color: var(--color-text);
    position: relative;
    transition: color 0.2s ease;
    position: relative;

    .theme-dark &
    {
      border-top: 1px dashed var(--color-line);
    }

    &:hover > .ui-tree-item-actions
    {
      opacity: 1;
    }

    &.is-disabled
    {
      cursor: not-allowed;
      opacity: .5;
    }

    &.is-active:before, &.is-selected:before, &:hover:before
    {
      content: ' ';
      position: absolute;
      top: 0;
      bottom: 0;
      left: 0;
      right: 0;
      background: var(--color-tree-selected);
    }

    &.is-selected:after
    {
      font-family: "Feather";
      content: "\e83e";
      font-size: 16px;
      color: var(--color-primary);
      z-index: 2;
    }

    &.is-selected .ui-tree-item-text
    {
      font-weight: bold;
    }
  }

  .ui-tree-header + .ui-tree-item
  {
    margin-top: -8px;
  }

  .ui-tree-item-link
  {
    display: grid;
    grid-template-columns: 28px 1fr auto;
    gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;    
    color: var(--color-text);

    &:hover
    {
      color: var(--color-text);
    }

    &.is-active
    {
      color: var(--color-text);
      font-weight: bold;
      //color: var(--color-primary);
    }
  }

  .ui-tree-item-text
  {
    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
  }

  .ui-tree-item-description
  {
    color: var(--color-text-dim);
    font-size: var(--font-size-xs);
  }

  .ui-tree-item-toggle
  {
    color: var(--color-text-dim);
    height: 100%;
    text-align: right;
    padding-right: 4px;
    transition: color 0.2s ease;
    z-index: 1;

    &:hover
    {
      color: var(--color-text);
    }
  }

  .ui-tree-item-icon
  {
    position: relative;
    top: -2px;
    color: var(--color-text-dim);
    transition: color 0.2s ease;
  }

  .ui-tree-item-icon.is-dashed
  {
    stroke-dasharray: 3.5px; 
  }

  .ui-tree-item:hover .ui-tree-item-icon
  {
    color: var(--color-text);
  }

  .ui-tree-item.is-active .ui-tree-item-icon
  {
    color: var(--color-text);
  }

  .ui-tree-item-loading
  {
    display: block;
    overflow: hidden;
    position: absolute;
    left: 0;
    right: 0;
    height: 2px;

    i
    {
      background-color: var(--color-bg-shade-4);
      transform: translateX(-100%) scaleX(1);
      animation: treeitemloading 1s linear infinite;
      width: 100%;
      height: 100%;
      position: absolute;
      left: 0;
      top: 0;
    }
  }

  @keyframes treeitemloading
  {
    0%  { transform: translateX(-100%); }
    100% { transform: translateX(100%); }
  }

  .ui-tree-item-modifier
  {
    position: absolute;
    left: 10px;
    bottom: 17px;
    color: var(--color-text-dim);
    background: var(--color-tree);
    border-radius: 50%;
    padding: 2px; 
    width: 14px;
    height: 14px; 
    transition: color 0.2s ease;

    .ui-tree-item.is-active &, .ui-tree-item:hover &
    {
      color: var(--color-text);
    }

    .ui-tree-item.is-active &
    {
      background: var(--color-tree-selected); 
    }
  }

  .ui-tree-item-actions
  {
    opacity: 0;
    color: var(--color-text-dim);

    &:focus
    {
      opacity: 1;
    }
  }

  .ui-tree-item-count
  {
    display: inline-block;
    font-size: 11px;
    font-weight: 400;
    text-transform: uppercase;
    background: var(--color-box-nested);
    color: var(--color-text);
    height: 22px;
    line-height: 22px;
    padding: 0 10px;
    border-radius: 16px;
    font-style: normal;
    grid-column: 3;
    margin-right: -4px;
    margin-left: 8px;
  }
</style>
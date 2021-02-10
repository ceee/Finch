<template>
  <div v-if="area" class="ui-linkpicker-area-pages">
    <ui-property :vertical="true">
      <ui-search v-model="search" />
    </ui-property>
    <ui-property :vertical="true">
      <ui-tree ref="tree" v-bind="treeConfig" :get="getTreeItems" @select="onSelect" :selection="selection" class="ui-linkpicker-area-pages-tree" />
    </ui-property>
  </div>
</template>


<script>
  import PageTreeApi from 'zero/api/page-tree.js';
  import { debounce as _debounce } from 'underscore';

  export default {
    name: 'uiLinkpickerAreaPages',

    props: {
      value: {
        type: Object,
        required: true
      },
      area: {
        type: Object,
        required: true
      }
    },

    data: () => ({
      selection: [],
      treeConfig: {
        parent: null,
        active: null,
        mode: 'select'
      },
      search: null,
      debouncedSearch: null
    }),

    watch: {
      search()
      {
        this.debouncedSearch();
      },
      value()
      {
        this.selection = [this.value.values.id];
      }
    },

    mounted()
    {
      this.debouncedSearch = _debounce(() => this.$refs.tree.refresh(), 300);
      this.selection = this.value.values.id ? [this.value.values.id] : [];
    },

    methods: {

      isValid()
      {
        return this.selection.length > 0;
      },

      onSelect(id)
      {
        if (id)
        {
          this.selection = [id];
          this.value.values = { id };
        }
        else
        {
          this.selection = [];
          this.value.values = { id: null };
        }
        this.$emit('change', this.value);
        this.$emit('input', this.value);
      },

      getTreeItems(parent)
      {
        return PageTreeApi.getChildren(parent, null, this.search).then(res =>
        {
          res = res.filter(x => x.id !== 'recyclebin');

          res.forEach(item =>
          {
            item.hasActions = false;
          });

          return res;
        });
      },
    }
  }
</script>

<style>
  .ui-linkpicker-area-pages-tree
  {
    margin: 0 -32px 0;  
  }

  .ui-linkpicker-area-pages-tree .ui-tree-item.is-selected, 
  .ui-linkpicker-area-pages-tree .ui-tree-item:hover:not(.is-disabled)
  {
    background: var(--color-tree-selected);
  }

  .ui-linkpicker-area-pages-tree
  {
    .ui-tree-item.is-disabled
    {
      opacity: .5;
    }

    .ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)
    {
      background: var(--color-tree-selected);
    }

    .ui-tree-item.is-selected
    {
      &:after
      {
        font-family: "Feather";
        content: "\e83e";
        font-size: 16px;
        color: var(--color-primary);
      }
      
      .ui-tree-item-text
      {
        font-weight: bold;
      }
    }
  }
</style>
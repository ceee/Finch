<template>
  <div v-if="area" class="ui-linkpicker-area-pages">
    <ui-property :vertical="true">
      <ui-search v-model="search" />
    </ui-property>
    <ui-property :vertical="true">
      <ui-tree ref="tree" v-bind="treeConfig" :get="getTreeItems" @select="onSelect" class="ui-linkpicker-area-pages-tree" />
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
      treeConfig: {
        parent: null,
        active: null
      },
      search: null,
      debouncedSearch: null
    }),

    watch: {
      search()
      {
        this.debouncedSearch();
      }
    },

    mounted()
    {
      this.debouncedSearch = _debounce(() => this.$refs.tree.refresh(), 300);
    },

    methods: {

      onSelect(item)
      {
        this.value.values = {
          id: item.id
        };
        this.$emit('change', this.value);
      },

      getTreeItems(parent)
      {
        return PageTreeApi.getChildren(parent, null, this.search).then(res =>
        {
          res = res.filter(x => x.id !== 'recyclebin');

          res.forEach(item =>
          {
            //if (item.id === this.model)
            //{
            //  item.isSelected = true;
            //}
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
</style>
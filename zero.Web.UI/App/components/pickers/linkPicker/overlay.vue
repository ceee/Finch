<template>
  <ui-overlay-editor class="ui-linkpicker-overlay">
    <template v-slot:header>
      <ui-header-bar :title="config.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" :label="config.closeLabel" :parent="config.rootId" @click="config.hide" />
    </template>

    <div v-if="opened">
      <!--<div class="ui-box ui-linkpicker-overlay-options">
        <ui-property label="Open in a new tab">
          <ui-toggle :value="link.target === 'blank'" @input="onTargetChange" />
        </ui-property>
        <template v-if="showOptions">
          <hr />
          <ui-property label="Title" :vertical="true">
            <input v-model="link.title" type="text" class="ui-input" maxlength="160" />
          </ui-property>
          <hr />
          <ui-button label="Hide options" @click="showOptions=false" />
        </template>
        <div v-if="!showOptions">
          <hr />
          <ui-button label="More options" @click="showOptions=true" caret="down" />
        </div>
      </div>-->
      <div class="ui-linkpicker-overlay-area">
        <ui-property :vertical="true">
          <ui-select v-model="current" :items="areaItems"></ui-select>
        </ui-property>
      </div>

      <div class="ui-box">
        <ui-property :vertical="true">
          <ui-search v-model="search" />
        </ui-property>
        <ui-property :vertical="true">
          <div class="ui-linkpicker-overlay-items" v-if="area">
            <ui-tree v-if="area.display === 'tree'" ref="tree" v-bind="treeConfig" :get="getTreeItems" @select="treeConfig.onSelect" />
          </div>
        </ui-property>
      </div>
    </div>
    <!--<div v-if="opened" class="ui-box ui-linkpicker-overlay-items">
      <ui-tree ref="tree" :get="getItems" :parent="config.rootId" @select="onSelect" />
    </div>-->
  </ui-overlay-editor>
</template>


<script>
  import PageTreeApi from 'zero/api/page-tree.js'
  import { debounce as _debounce } from 'underscore';

  export default {

    props: {
      model: String,
      config: Object
    },

    data: () => ({
      opened: false,
      current: null,
      area: null,
      areas: [],
      areaItems: [],
      treeConfig: {
        parent: null,
        active: null,
        onSelect: (ev) => { console.info(JSON.parse(JSON.stringify(ev))); }
      },
      search: null,
      link: null,
      template: {
        area: null,
        target: 'default',
        urlSuffix: null,
        title: null,
        values: {}
      },
      showOptions: false
    }),


    watch: {
      current()
      {
        this.reloadSelector();
      },
      search()
      {
        this.debouncedSearch();
      }
    },


    mounted()
    {
      this.debouncedSearch = _debounce(() => this.$refs.tree.refresh(), 300);

      this.areas = this.zero.config.linkPicker.areas;
      this.areaItems = this.areas.map(x =>
      {
        return {
          key: x.alias,
          value: x.name
        };
      });
      this.area = this.areas[0];
      this.current = this.area.alias;

      this.link = JSON.parse(JSON.stringify(this.template));
      this.link.area = this.current;

      setTimeout(() => this.opened = true, 300);
    },


    methods: {

      reloadSelector()
      {
        this.area = this.areas.find(x => x.alias === this.current);

        if (!this.opened)
        {
          return;
        }
        if (this.area.display === 'tree' && this.$refs.tree)
        {
          this.$refs.tree.refresh();
        }
        else if (this.area.display === 'list')
        {

        }
        else if (this.area.display === 'media')
        {

        }
        else
        {
          // custom
        }
      },

      onSelect(item)
      {
        this.config.confirm(item);
      },

      // get list items
      getListItems(search)
      {

      },

      // get tree items
      getTreeItems(parent)
      { 
        return PageTreeApi.getChildren(parent, null, this.search).then(res =>
        {
          res = res.filter(x => x.id !== 'recyclebin');
          
          res.forEach(item =>
          {
            if (item.id === this.model)
            {
              item.isSelected = true;
            }
            item.hasActions = false;
          });

          return res;
        });
      },


      onTargetChange(ev)
      {
        this.link.target = ev ? 'blank' : 'default';
      }
    }
  }
</script>

<style lang="scss">
  .ui-linkpicker-overlay-area
  {
    margin-bottom: var(--padding-s);
  }

  .ui-linkpicker-overlay .ui-property + .ui-property
  {
    margin-top: 15px;
  }

  .ui-linkpicker-overlay-area .ui-native-select
  {
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
    font-weight: bold;
  }

  .ui-linkpicker-overlay-area .ui-native-select select
  {
    font-weight: bold;
  }

  .ui-linkpicker-overlay-area .ui-native-select select option
  {
    font-weight: normal;
  }

  .ui-linkpicker-overlay content
  {
    padding-top: 0; 
  }

  .ui-linkpicker-overlay-options .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .ui-linkpicker-overlay-options .ui-property + .ui-property
  {
    margin-top: var(--padding-m); 
  }

  .ui-linkpicker-overlay-options .ui-property-content
  {
    display: inline;
    flex: 0 0 auto; 
  }

  .ui-linkpicker-overlay-options .ui-property-label
  {
    padding-top: 1px;
  }

  .ui-linkpicker-overlay-items > .ui-tree
  {
    margin: 0 -32px 0;
  }

  .ui-linkpicker-overlay-items .ui-tree-item.is-selected, .ui-linkpicker-overlay-items .ui-tree-item:hover:not(.is-disabled)
  {
    background: var(--color-tree-selected);
  }

  /*.ui-linkpicker-overlay-items .ui-tree-item.is-selected
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
  }*/
</style>
<template>
  <ui-overlay-editor class="pages-copy">
    <template v-slot:header>
      <ui-header-bar title="@ui.copy.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="white" label="@ui.close" @click="config.hide"></ui-button>
      <ui-button label="@ui.copy.action" @click="onSave" :state="state" :disabled="selected == null"></ui-button>
    </template>

    <p class="pages-copy-text" v-localize:html="{ key: '@ui.copy.text', tokens: { name: model.name } }"></p>
    <div class="ui-box">
      <ui-property label="Include descendants">
        <ui-toggle v-model="includeDescendants" class="is-primary" />
      </ui-property>
    </div>
    <div class="ui-box pages-copy-items">
      <ui-tree ref="tree" :get="getItems" @select="onSelect" />
    </div>
  </ui-overlay-editor>
</template>


<script>
  import PageTreeApi from 'zero/resources/page-tree.js'
  import PagesApi from 'zero/resources/pages';
  import Arrays from 'zero/services/arrays.js'

  export default {

    props: {
      isCopy: {
        type: Boolean,
        default: false
      },
      model: Object,
      config: Object
    },

    data: () => ({
      items: [],
      selected: [],
      state: 'default',
      cache: {},
      prevItem: null,
      selected: null,
      includeDescendants: true
    }),


    mounted()
    {
      
    },


    methods: {

      onSelect(item)
      {
        item.isSelected = true;

        if (this.prevItem && this.prevItem.id != item.id)
        {
          this.prevItem.isSelected = false;
        }

        this.prevItem = item;
        this.selected = item;
        //this.config.confirm(item);
      },

      getItems(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return Promise.resolve(this.cache[key]);
        }

        return PageTreeApi.getChildren(parent).then(response =>
        {
          if (!parent)
          {
            response.splice(0, 0, {
              id: null,
              parentId: null,
              sort: 0,
              name: 'Root',
              icon: 'fth-arrow-down-circle',
              isOpen: false,
              modifier:	null,
              hasChildren: false,
              childCount: 0,
              isInactive: false,
              hasActions: false
            });
          }

          response.forEach(item =>
          {
            //item.disabled = true;
            item.isSelected = false;
            item.disabled = item.id === 'recyclebin' || item.id == this.model.id;
            item.hasActions = false;
          });

          this.cache[key] = response;
          return response;
        });
      },

      onSave()
      {
        this.state = 'loading';

        PagesApi.copy(this.model.id, this.selected.id, this.includeDescendants).then(res =>
        {
          if (res.success)
          {
            this.state = 'success';
            this.config.confirm(res.model);
          }
          else
          {
            this.state = 'error';
          }
        });
      }
    }
  }
</script>

<style lang="scss">
  .pages-copy .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .pages-copy .ui-property-content
  {
    display: inline;
    flex: 0 0 auto;
  }

  .pages-copy .ui-property-label
  {
    padding-top: 1px;
  }

  .pages-copy .ui-box
  {
    margin: 0;    
    padding: 20px var(--padding) 18px;

    & + .ui-box
    {
      padding: 16px 0;
      margin-top: 26px;
    }

    .ui-tree-item.is-disabled
    {
      opacity: .5;
    }

    .ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)
    {
      background: var(--color-bg-xxlight);
    }

    .ui-tree-item.is-selected
    {
      &:after
      {
        font-family: "Feather";
        content: "\e83e";
        font-size: 16px;
        color: var(--color-secondary);
      }
      
      .ui-tree-item-text
      {
        font-weight: bold;
      }
    }
  }

  .pages-copy content
  {
    padding-top: 0;
  }

  .pages-copy-text
  {
    margin: 0 0 20px;
  }
</style>
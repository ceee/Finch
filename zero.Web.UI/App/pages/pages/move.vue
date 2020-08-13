<template>
  <ui-overlay-editor class="pages-move">
    <template v-slot:header>
      <ui-header-bar title="@ui.move.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="white" label="@ui.close" @click="config.hide"></ui-button>
      <ui-button label="@ui.move.action" @click="onSave" :state="state"></ui-button>
    </template>

    <p class="pages-move-text" v-localize:html="{ key: '@ui.move.text', tokens: { name: model.name } }"></p>
    <div class="ui-box pages-move-items">
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
      selected: null
    }),


    mounted()
    {
      this.selected = this.model;
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
            item.isSelected = this.model.parentId == item.id;

            if (item.isSelected)
            {
              this.prevItem = item;
            }
            item.disabled = item.id === 'recyclebin' || item.id == this.model.id;
            item.hasActions = false;
          });

          this.cache[key] = response;
          return response;
        });
      },

      onSave()
      {
        if (this.model.parentId == this.selected.id)
        {
          this.config.close();
          return;
        }

        this.state = 'loading';

        PagesApi.move(this.model.id, this.selected.id).then(res =>
        {
          if (res.success)
          {
            this.state = 'success';
            this.config.confirm();
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
  .pages-move .ui-box
  {
    margin: 0;
    padding: 16px 0;

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

  .pages-move content
  {
    padding-top: 0;
  }

  .pages-move-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto;
    grid-gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    height: 42px;
    color: var(--color-fg);
    position: relative;
    padding: 0 8px;
    background: var(--color-bg-light);
    border-radius: var(--radius);

    i
    {
      font-size: var(--font-size-l);
      position: relative;
      top: -1px;
      color: var(--color-fg-light);
    }

    span
    {
      padding: 12px 8px;
    }

    &.is-selected
    {
      color: var(--color-fg-light);
    }

    & + .pages-move-item
    {
      margin-top: 8px;
    }
  }

  button.pages-move-item-button
  {
    height: 42px;
    width: 24px;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;

    i
    {
      font-size: var(--font-size);
    }

    &.is-handle
    {
      cursor: move;
    }
  }

  .pages-move-text
  {
    margin: 0 0 20px;
  }

  i.pages-move-item-icon
  {
    top: 0;
    color: var(--color-fg);
    margin-right: 8px;
  }
</style>
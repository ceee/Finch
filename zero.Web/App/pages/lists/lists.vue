<template>
  <div class="lists">
    <div class="lists-tree" v-resizable="resizable">
      <ui-header-bar title="Lists" :back-button="false" />
      <div class="lists-tree-items">
        <div v-for="item in lists" class="lists-tree-item" :class="getClasses(item)">
          <router-link :to="{ name: 'list', params: { alias: item.alias } }" class="lists-tree-item-link">
            <i class="lists-tree-item-icon" :class="item.icon"></i>
            <span class="lists-tree-item-text">
              {{item.name | localize}}
              <span v-if="item.description" v-localize="item.description"></span>
            </span>
          </router-link>
          <ui-dot-button class="lists-tree-item-actions" />
        </div>
      </div>
      <!--<div class="lists-tree-actions">
        <ui-button label="Add list" icon="fth-plus" />
      </div>-->
      <div class="lists-tree-resizable ui-resizable"></div>
    </div>

    <router-view v-if="!isOverview"></router-view>

    <div v-if="isOverview" class="lists-overview">
      
    </div>

  </div>
</template>

<script>
  import ListsApi from 'zero/resources/lists.js';

  export default {
    data: () => ({
      tableConfig: {},
      lists: [],
      resizable: {
        axis: 'x',
        min: 260,
        max: 520,
        save: 'lists-tree',
        handle: '.ui-resizable'
      }
    }),

    computed: {
      isOverview()
      {
        return !this.$route.params.id && this.$route.name !== 'list' && this.$route.name !== 'listitem';
      }
    },

    created()
    {
      ListsApi.getCollections().then(response =>
      {
        this.lists = response;
      });
      //this.tableConfig = {
      //  labelPrefix: '@lists.fields.',
      //  allowOrder: false,
      //  search: null,
      //  columns: {
      //    name: 'text'
      //  },
      //  items: ListsApi.getCollections
      //};
    },


    methods: {
      // get all classes for a tree item
      getClasses(item)
      {
        return {
          'has-children': item.hasChildren,
          'is-open': item.isOpen
        };
      }
    }
  }
</script>


<style lang="scss">
  .lists
  {
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 2px;
    justify-content: stretch;
    height: 100vh;
  }

  .lists-tree
  {
    width: 340px;
    background: var(--color-bg-light);
    padding: 0;
    position: relative;
    overflow-y: auto;
    height: 100vh;

    .ui-header-bar + .ui-tree
    {
      margin-top: 2px;
    }
  }

  .lists-tree-actions
  {
    padding: var(--padding);
  }

  .lists-tree-resizable
  {
    position: absolute;
    top: 0;
    bottom: 0;
    background: var(--color-fg);
    opacity: 0;
    right: 0;
    width: 6px;
    cursor: ew-resize;
    transition: opacity 0.15s ease 0s;

    &:hover
    {
      transition-delay: 0.2s;
      opacity: 0.04;
    }
  }

  .lists-overview
  {
    padding: 95px 0 0 60px;
  }


  .lists-tree-item
  {
    display: grid;
    grid-template-columns: 1fr auto;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 var(--padding);
    height: 50px;
    color: var(--color-fg);
    position: relative;
    transition: color 0.2s ease;

    &:hover > .lists-tree-item-actions
    {
      transition-delay: 0.2s;
      opacity: 1;
    }

    & + .lists-tree-item
    {
      margin-top: 15px;
    }
  }

  .lists-tree-item-link
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    grid-gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;
    color: var(--color-fg);

    &.is-active
    {
      font-weight: bold;
    }
  }

  .lists-tree-item-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-fg-light);
    }
  }

  .lists-tree-item-toggle
  {
    position: absolute;
    color: var(--color-fg-mid);
    height: 100%;
    top: 0;
    left: 0;
    width: 30px;
    text-align: right;
    padding-right: 5px;
    outline: none !important;
    transition: color 0.2s ease;

    &:hover
    {
      color: var(--color-fg);
    }
  }

  .lists-tree-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-fg-reverse-mid);
    transition: color 0.2s ease;
  }

  .lists-tree-item-actions
  {
    transition: opacity 0.2s ease 0;
    opacity: 0;
    color: var(--color-fg-mid);
  }
</style>
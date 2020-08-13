<template>
  <div class="page-container">
    <div class="page-container-tree" v-resizable="resizable">
      <ui-tree ref="tree" :get="getItems" :config="treeConfig" :active="id" header="Pages" />
      <div class="page-container-tree-resizable ui-resizable"></div>
    </div>

    <router-view v-if="!isOverview"></router-view>

    <div v-if="isOverview" class="page-overview">
      <button type="button" @click="action.action(action)" v-for="action in actions" :key="action.alias" class="page-overview-action">
        <i class="page-overview-action-icon" :class="action.icon" />
        <p class="page-overview-action-text">
          <strong v-localize="'@page.overview.actions.' + action.alias"></strong>
          <br>
          <span v-localize="{ key: '@page.overview.actions.' + action.alias + '_text', tokens: action.tokens }"></span>
        </p>
      </button>
    </div>
  </div>
</template>


<script>
  import PageTreeApi from 'zero/resources/page-tree.js'
  import Overlay from 'zero/services/overlay.js'
  import CreateOverlay from './create'
  import SortOverlay from './sort'
  import EventHub from 'zero/services/eventhub'

  export default {

    data: () => ({
      page: true,
      cache: {},
      resizable: {
        axis: 'x',
        min: 260,
        max: 520,
        save: 'page-tree',
        handle: '.ui-resizable'
      },
      actions: [],
      treeConfig: {

      }
    }),


    computed: {
      id()
      {
        return this.$route.params.id;
      },
      isOverview()
      {
        return !this.$route.params.id && !this.$route.params.type && this.$route.name !== 'recyclebin';
      }
    },


    created()
    {
      var instance = this;

      this.actions.push({
        alias: 'continue',
        icon: 'fth-corner-down-right',
        tokens: {
          page: 'Products',
          date: 'March 3rd, 2020'
        }
      });
      this.actions.push({
        alias: 'new',
        icon: 'fth-plus',
        tokens: {
          root: 'Home'
        },
        action()
        {
          instance.create();
        }
      });
      this.actions.push({
        alias: 'history',
        icon: 'fth-clock'
      });

      this.treeConfig.onActionsRequested = item =>
      {
        let actions = [];

        if (item && item.id === 'recyclebin')
        {
          return [{
            name: 'Empty recycle bin',
            icon: 'fth-trash-2'
          }];
        }

        actions.push({
          name: 'Create',
          icon: 'fth-plus',
          action(action, dropdown)
          {
            dropdown.hide();
            instance.create(item);
          }
        });

        if (item)
        {
          actions.push({
            name: 'Move',
            icon: 'fth-corner-down-right'
          });
          actions.push({
            name: 'Copy',
            icon: 'fth-copy',
            disabled: true
          });
        }

        actions.push({
          name: 'Sort',
          icon: 'fth-arrow-down',
          action(action, dropdown)
          {
            dropdown.hide();
            instance.sort(item);
          }
        });

        if (item)
        {
          actions.push({
            type: 'separator'
          });
          actions.push({
            name: 'Delete',
            icon: 'fth-x'
          });
        }

        return actions;
      };

      EventHub.$on('page.update', page =>
      {
        this.cache = [];
        this.$refs.tree.refresh();
      });
    },


    methods: {

      getItems(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return Promise.resolve(this.cache[key]);
        }

        return PageTreeApi.getChildren(parent, this.id).then(response =>
        {
          response.forEach(item =>
          {
            item.url = {
              name: 'page',
              params: { id: item.id }
            };

            if (item.id === "recyclebin")
            {
              item.url = {
                name: 'recyclebin'
              };
            }
          });
          this.cache[key] = response;
          return response;
        });
      },


      // opens dialog for creating a new page
      create(parent)
      {
        Overlay.open({
          component: CreateOverlay,
          width: 520,
          parent: parent
        }).then(() =>
        {

        }, () =>
        {
                
        });
      },


      sort(item)
      {
        return Overlay.open({
          component: SortOverlay,
          display: 'editor',
          model: item
        }).then(value =>
        {
          EventHub.$emit('page.update');
        });
      }
    }
  }
</script>


<style lang="scss">
  .page-container
  {
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 2px;
    justify-content: stretch;
    height: 100vh;
  }

  .page-container-tree
  {
    width: 340px;
    background: var(--color-bg-light);
    padding: 0;
    position: relative;
    overflow-y: auto;
    height: 100vh;
    border-right: 1px solid var(--color-line-mid);

    .ui-header-bar + .ui-tree
    {
      margin-top: -10px;
    }

    .ui-dot-button
    {
      margin-right: -8px;
    }
  }

  .page-container-tree-resizable
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

  .page-overview
  {
    padding: 95px 0 0 60px;
  }

  .page-overview-action
  {
    color: var(--color-text);
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 35px;
    align-items: center;

    & + .page-overview-action
    {
      margin-top: 60px;
    }
  }

  .page-overview-action-icon
  {
    width: 90px;
    height: 90px;
    line-height: 89px !important;
    font-size: 22px;
    text-align: center;
    background: var(--color-bg-light);
    border-radius: var(--radius);
    transition: box-shadow 0.2s ease;
    box-shadow: var(--color-shadow-short);
  }

  .page-overview-action-text
  {
    line-height: 1.3;
    color: var(--color-fg-light);

    strong
    {
      display: inline-block;
      margin-bottom: 8px;
      color: var(--color-fg);
      font-size: var(--font-size-l);
    }
  }
</style>
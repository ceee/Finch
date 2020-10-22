<template>
  <div class="page-container">
    <div class="app-tree" v-resizable="resizable">
      <ui-tree ref="tree" :get="getItems" :config="treeConfig" :active="id" header="Pages">
        <template v-slot:actions="props">
          <template v-if="!props.item || props.id !== 'recyclebin'">
            <ui-dropdown-button label="@ui.create" icon="fth-plus" @click="create(props.item)" />
            <ui-dropdown-button v-if="props.item" label="@ui.move.title" icon="fth-corner-down-right" @click="move(props.item)" />
            <ui-dropdown-button v-if="props.item" label="@ui.copy.title" icon="fth-copy" @click="copy(props.item)" />
            <ui-dropdown-button label="@ui.sort.title" icon="fth-arrow-down" @click="sort(props.item)" />
            <ui-dropdown-separator v-if="props.item" />
            <ui-dropdown-button v-if="props.item" label="@ui.delete" icon="fth-trash" @click="remove(props.item)" />
          </template>
        </template>
      </ui-tree>
      <div class="app-tree-resizable ui-resizable"></div>
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
  import PagesApi from 'zero/resources/pages.js'
  import Overlay from 'zero/services/overlay.js'
  import CreateOverlay from './overlays/create'
  import SortOverlay from './overlays/sort'
  import MoveOverlay from './overlays/move'
  import CopyOverlay from './overlays/copy'
  import EventHub from 'zero/services/eventhub'
  import Notification from 'zero/services/notification.js'
  import Strings from 'zero/services/strings';

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
    

    mounted()
    {
      this.buildActions();
      EventHub.$off('page.update');
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
              item.hasActions = false;
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
          EventHub.$emit('page.sort', value);
          EventHub.$emit('page.update');
        });
      },


      move(item)
      {
        return Overlay.open({
          component: MoveOverlay,
          display: 'editor',
          model: item
        }).then(value =>
        {
          EventHub.$emit('page.move', value);
          EventHub.$emit('page.update');
        });
      },


      copy(item)
      {
        console.info(item);
        return Overlay.open({
          component: CopyOverlay,
          display: 'editor',
          model: item
        }).then(value =>
        {
          EventHub.$emit('page.update');
          this.$router.push({
            name: 'page',
            params: { id: value.id }
          });
        });
      },


      remove(item)
      {
        Overlay.confirmDelete(item.name, '@deleteoverlay.page_text').then(opts =>
        {
          opts.state('loading');

          PagesApi.delete(item.id).then(response =>
          {
            if (response.success)
            {
              opts.state('success');
              opts.hide();
              EventHub.$emit('page.delete', response.model);
              EventHub.$emit('page.update');
              Notification.success('@deleteoverlay.success', '@deleteoverlay.page_success_text');
            }
            else
            {
              opts.errors(response.errors);
            }
          });
        });
      },


      buildActions()
      {
        this.actions = [];

        var instance = this;

        let lastEditedPageId = localStorage.getItem('zero.last-page.' + zero.appId);

        if (lastEditedPageId)
        {
          PagesApi.getById(lastEditedPageId).then(res =>
          {
            this.actions.push({
              alias: 'continue',
              icon: 'fth-corner-down-right',
              tokens: {
                page: res.entity.name
              },
              action()
              {
                instance.$router.push({
                  name: 'page',
                  params: { id: res.entity.id }
                });
              }
            });
          });
        }


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
          icon: 'fth-clock',
          action()
          {
            Notification.error('Not implemented', 'Page editing history has not been implemented yet');
          }
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
    gap: 2px;
    justify-content: stretch;
    height: 100vh;
  }


  .page-overview
  {
    display: flex;
    flex-direction: column;
    margin-left: 80px;
    padding-top: 115px;
  }

  .page-overview-action
  {
    color: var(--color-text);
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 35px;
    align-items: center;

    & + .page-overview-action
    {
      margin-top: var(--padding);
    }
  }

  .page-overview-action-icon
  {
    width: 90px;
    height: 90px;
    line-height: 89px !important;
    font-size: 22px;
    text-align: center;
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
  }

  .page-overview-action-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);

    strong
    {
      display: inline-block;
      margin-bottom: 8px;
      color: var(--color-text);
      font-size: var(--font-size-l);
    }
  }
</style>
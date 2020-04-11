<template>
  <div class="page-container">
    <div class="page-container-tree" v-resizable="resizable">
      <ui-header-bar title="Pages">
        <ui-dot-button />
      </ui-header-bar>
      <ui-tree :get="getItems" />
      <div class="page-container-tree-resizable ui-resizable"></div>
    </div>

    <router-view v-if="!isOverview"></router-view>

    <div v-if="isOverview" class="page-overview">
      <router-link :to="action.url" v-for="action in actions" :key="action.alias" class="page-overview-action">
        <i class="page-overview-action-icon" :class="action.icon" />
        <p class="page-overview-action-text">
          <strong v-localize="'@page.overview.actions.' + action.alias"></strong>
          <br>
          <span v-localize="{ key: '@page.overview.actions.' + action.alias + '_text', tokens: action.tokens }"></span>
        </p>
      </router-link>
    </div>
  </div>
</template>


<script>
  import PageTreeApi from 'zero/resources/page-tree.js'

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
      actions: []
    }),


    computed: {
      isOverview()
      {
        return !this.$route.params.id;
      }
    },


    created()
    {
      this.actions.push({
        alias: 'continue',
        icon: 'fth-corner-down-right',
        url: '/',
        tokens: {
          page: 'Products',
          date: 'March 3rd, 2020'
        }
      });
      this.actions.push({
        alias: 'new',
        icon: 'fth-plus',
        url: '/',
        tokens: {
          root: 'Home'
        }
      });
      this.actions.push({
        alias: 'history',
        icon: 'fth-clock',
        url: '/'
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

        return PageTreeApi.getChildren(parent).then(response =>
        {
          response.forEach(item =>
          {
            item.url = {
              name: 'page',
              params: { id: item.id }
            }
          });
          this.cache[key] = response;
          return response;
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

    .ui-header-bar + .ui-tree
    {
      margin-top: 2px;
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

  a.page-overview-action
  {
    color: var(--color-text);
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 35px;
    align-items: center;

    &:hover .page-overview-action-icon
    {
      box-shadow: 0 0 20px var(--color-shadow);
    }

    & + a.page-overview-action
    {
      margin-top: 60px;
    }
  }

  .page-overview-action-icon
  {
    width: 100px;
    height: 100px;
    line-height: 98px !important;
    font-size: 26px;
    text-align: center;
    background: var(--color-bg-light);
    border-radius: var(--radius);
    transition: box-shadow 0.2s ease;
    box-shadow: 0 0 20px transparent;
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
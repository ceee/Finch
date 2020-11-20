<template>
  <div class="spaces">
    <div class="app-tree spaces-tree" v-resizable="resizable">
      <ui-header-bar title="@space.list" />

      <div class="spaces-tree-items">
        <router-link :to="{ name: 'space', params: { alias: item.alias } }" v-for="item in spaces" :key="item.alias" class="spaces-tree-item" :class="{ 'has-line': item.lineBelow }">
          <i class="spaces-tree-item-icon" :class="item.icon"></i>
          <span class="spaces-tree-item-text">
            <span v-localize="item.name"></span>
            <span class="-minor" v-if="item.description" v-localize="item.description"></span>
          </span>
        </router-link>
      </div>
      <div class="spaces-tree-resizable ui-resizable"></div>
    </div>

    <component v-if="!isOverview && loaded && component" ref="comp" :is="component" :space="space" :config="spaceConfig"></component>
  </div>
</template>

<script>
  import SpaceEditor from 'zero/pages/spaces/views/editor.vue';
  import SpaceList from 'zero/pages/spaces/views/list.vue';
  import SpaceCustom from 'zero/pages/spaces/views/custom.vue';
  import SpacesApi from 'zero/api/spaces.js';

  export default {
    data: () => ({
      spaces: [],
      loaded: false,
      component: null,
      space: null,
      spaceConfig: {},
      resizable: {
        axis: 'x',
        min: 260,
        max: 520,
        save: 'spaces-tree',
        handle: '.ui-resizable'
      }
    }),

    computed: {
      isOverview()
      {
        return !this.$route.params.alias;
      }
    },

    watch: {
      '$route': 'loadSpace'
    },

    created()
    {
      SpacesApi.getAll().then(response =>
      {
        this.spaces = response;
        this.loadSpace();
      });
    },

    beforeRouteLeave(to, from, next) 
    {
      if (this.$refs.comp && this.$refs.comp.beforeRouteLeave)
      {
        this.$refs.comp.beforeRouteLeave(to, from, next);
      }
      else
      {
        next();
      }
    },

    beforeRouteUpdate(to, from, next)
    {
      if (this.$refs.comp && this.$refs.comp.beforeRouteLeave)
      {
        this.$refs.comp.beforeRouteLeave(to, from, next);
      }
      else
      {
        next();
      }
    },

    methods: {

      loadSpace()
      {
        this.loaded = false;

        this.$nextTick(() =>
        {
          if (this.isOverview)
          {
            this.space = null;
            this.component = null;
            this.loaded = true;
            return;
          }

          this.space = this.spaces.find(space => space.alias === this.$route.params.alias);

          if (this.space.view === 'editor' || this.$route.params.id || this.$route.meta.create)
          {
            this.component = SpaceEditor;
          }
          else if (this.space.view === 'list')
          {
            this.component = SpaceList;
          }
          else
          {
            throw "Not implemented. Custom space view";
          }

          this.loaded = true;
        });
      }
    }
  }
</script>


<style lang="scss">
  .spaces
  {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 2px;
    justify-content: stretch;
    height: 100vh;
  }

  .spaces-overview
  {
    padding: 95px 0 0 60px;
  }

  .spaces-tree-items
  {
    margin-top: -13px;
  }

  .spaces-tree-item
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;
    padding: 15px var(--padding);
    font-size: var(--font-size);
    color: var(--color-text);
    transition: color 0.2s ease;

    &:hover > .spaces-tree-item-actions
    {
      transition-delay: 0.2s;
      opacity: 1;
    }

    &.is-active
    {
      background: var(--color-tree-selected);
      font-weight: bold;

      .spaces-tree-item-text span
      {
        font-weight: 400;
      }

      .spaces-tree-item-text span:first-child
      {
        font-weight: 700;
      }
    }

    &.is-active, &:hover
    {
      .spaces-tree-item-icon
      {
        color: var(--color-text);
      }
    }
  }

  .spaces-tree-item-text
  {
    display: flex;
    flex-direction: column;

    .-minor
    {
      color: var(--color-text-dim);
      margin-top: 3px;
    }
  }

  .spaces-tree-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-text-dim);
    transition: color 0.2s ease;
  }
</style>
<template>
  <div class="spaces">
    <div class="app-tree spaces-tree" xv-resizable="resizable">
      <ui-header-bar title="@space.list" />

      <div class="spaces-tree-items">
        <router-link :to="{ name: 'spaces-view', params: { alias: item.alias } }" v-for="item in spaces" :key="item.alias" class="spaces-tree-item" :class="{ 'has-line': item.lineBelow }">
          <ui-icon class="spaces-tree-item-icon" :symbol="item.icon" />
          <span class="spaces-tree-item-text">
            <span v-localize="item.name"></span>
            <span class="-minor" v-if="item.description" v-localize="item.description"></span>
          </span>
        </router-link>
      </div>
      <div class="spaces-tree-resizable ui-resizable"></div>
    </div>

    <component class="spaces-main" v-if="!isOverview && loaded && component" ref="comp" :is="component" :space="space" :config="spaceConfig"></component>
  </div>
</template>


<script lang="ts">
  import api from './api';
  import EditorComponent from './views/editor.vue';
  import ListComponent from './views/list.vue';

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


    async created()
    {
      const types = await api.getTypes();
      this.spaces = types.data;
      this.loadSpace();
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
        if (!this.$route.name || this.$route.name.indexOf('space') !== 0)
        {
          return;
        }

        this.loaded = false;

        this.$nextTick(() =>
        {
          if (this.isOverview)
          {
            const space = this.spaces[0];

            if (space)
            {
              this.$router.replace({
                name: 'spaces-view',
                params: { alias: space.alias }
              });
            }
            else
            {
              this.space = null;
              this.component = null;
              this.loaded = true;
            }
            return;
          }

          this.space = this.spaces.find(space => space.alias === this.$route.params.alias);

          if (this.space.view === 1 || this.$route.params.id || this.$route.meta.create)
          {
            this.component = EditorComponent;
          }
          else if (this.space.view === 0)
          {
            this.component = ListComponent;
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

  .spaces-main
  {
    min-height: 100vh;
    overflow-y: auto;
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
    grid-template-columns: 32px 1fr auto;
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
    /*&.is-active:before
    {
      content: '';
      position: absolute;
      left: 0;
      top: 0;
      bottom: 0;
      width: 3px;
      display: inline-block;
      background: var(--color-tree-selected-line);
    }*/

    &:hover .spaces-tree-item-icon
    {
      color: var(--color-text);
    }

    &.is-active .spaces-tree-item-icon
    {
      color: var(--color-primary);
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
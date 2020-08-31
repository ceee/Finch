<template>
  <div class="spaces">
    <div class="app-tree spaces-tree" v-resizable="resizable">
      <ui-header-bar title="Spaces" />
      <div class="spaces-tree-items">
        <div v-for="item in spaces" class="spaces-tree-item" :class="getClasses(item)">
          <router-link :to="{ name: 'space', params: { alias: item.alias } }" class="spaces-tree-item-link">
            <i class="spaces-tree-item-icon" :class="item.icon"></i>
            <span class="spaces-tree-item-text">
              {{item.name | localize}}
              <span v-if="item.description" v-localize="item.description"></span>
            </span>
          </router-link>
          <ui-dot-button class="spaces-tree-item-actions" />
        </div>
      </div>
      <!--<div class="spaces-tree-actions">
        <ui-button label="Add list" icon="fth-plus" />
      </div>-->
      <div class="spaces-tree-resizable ui-resizable"></div>
    </div>

    <!--<router-view v-if="!isOverview"></router-view>-->
    <component v-if="!isOverview && loaded && component" ref="comp" :is="component" :space="space" :config="spaceConfig"></component>

    <div v-if="isOverview" class="spaces-overview">
      
    </div>

  </div>
</template>

<script>
  import SpaceEditor from 'zero/pages/spaces/views/editor';
  import SpaceList from 'zero/pages/spaces/views/list';
  import SpaceCustom from 'zero/pages/spaces/views/custom';
  import SpacesApi from 'zero/resources/spaces.js';
  import { find as _find } from 'underscore';

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
        if (this.isOverview)
        {
          this.space = null;
          this.component = null;
          this.loaded = true;
          return;
        }

        this.loaded = false;

        this.space = _find(this.spaces, space => space.alias === this.$route.params.alias);

        if (this.space.view === 'editor' || this.$route.params.id)
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
      },

      // get all classes for a tree item
      getClasses(item)
      {
        return {
          'has-children': item.hasChildren,
          'has-line': item.lineBelow,
          'is-open': item.isOpen,
          'is-active': item.alias == this.$route.params.alias
        };
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
    grid-template-columns: 1fr auto;
    align-items: center;
    font-size: var(--font-size);
    padding: 15px var(--padding);
    color: var(--color-fg);
    position: relative;
    transition: color 0.2s ease;

    &:hover > .spaces-tree-item-actions
    {
      transition-delay: 0.2s;
      opacity: 1;
    }

    /*&.has-line
    {
      margin-bottom: 25px;
    }

    &.has-line:after
    {
      content: '';
      position: absolute;
      bottom: -25px;
      left: 0;
      right: 0;
      border-bottom: 2px solid var(--color-bg);
    }*/

    &.is-active
    {
      background: var(--color-bg-bright-two);
      border-right: 3px solid var(--color-primary);
    }
  }

  .spaces-tree-item-link
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;
    color: var(--color-fg);

    &.is-active
    {
      font-weight: bold;

      .spaces-tree-item-text span
      {
        font-weight: 400;
      }
    }
  }

  .spaces-tree-item-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-fg-dim);
      margin-top: 3px;
    }
  }

  .spaces-tree-item-toggle
  {
    position: absolute;
    color: var(--color-fg-dim);
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

  .spaces-tree-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-fg-reverse-mid);
    transition: color 0.2s ease;
  }

  .spaces-tree-item-actions
  {
    transition: opacity 0.2s ease 0;
    opacity: 0;
    color: var(--color-fg-dim);
  }
</style>
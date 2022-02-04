<template>
  <div class="app-search">
    <form class="app-search-form" @submit.prevent="onSubmit">
      <input ref="input" class="app-search-form-input" type="search" v-model="query" @input="onSearch" v-localize:placeholder="'@search.input_placeholder'" />
      <ui-button class="app-search-submit" :submit="true" type="blank" icon="fth-search" />
    </form>
    <div v-if="list.items.length" class="app-search-items">
      <ui-link :to="item.url" v-for="item in list.items" :key="item.id" class="app-search-item" @click.native="config.close(true)">
        <ui-icon :symbol="item.icon" :size="18" class="app-search-item-icon" />
        <span class="app-search-item-text">
          {{item.name}}<span class="-minor" v-if="item.description"> <span>&ndash;</span> {{item.description}}</span>
        </span>
        <span class="app-search-item-group" v-localize="item.group"></span>
      </ui-link>
    </div>
  </div>
  <div class="app-search-hint">
    <p v-localize:html="'@search.native_hint'"></p>
    <label class="ui-native-check is-right app-search-shortcut onbg">
      <input type="checkbox" v-model="useShortcut" @change="onShortcutUpdated" />
      <span class="ui-native-check-toggle"></span>
      <p v-localize:html="'@search.native_toggle'"></p>
    </label>
  </div>
</template>


<script>
  import api from './api';
  import { debounce } from '../../utils';
  import { useSearchStore } from './store';

  export default {

    props: {
      model: Object,
      config: Object
    },

    name: 'app-search',

    data: () => ({
      open: false,
      query: null,
      list: {
        page: 1,
        totalPages: 1,
        items: []
      },
      onSearch: null,
      closeListener: null,
      useShortcut: true,
      store: null
    }),

    created()
    {
      this.onSearch = debounce(this.search, 300);
    },

    mounted()
    {
      this.store = useSearchStore();
      this.useShortcut = this.store.shortcutEnabled();

      this.$nextTick(() =>
      {
        this.$refs.input.focus();
        //this.$refs.input.select();
      });

      this.closeListener = e =>
      {
        let isSearchCombi = this.store.shortcutEnabled() && e.key === "f" && (e.ctrlKey || e.metaKey);
        if (e.key === "Escape" || isSearchCombi)
        {
          if (!isSearchCombi)
          {
            e.preventDefault();
          }
          this.config.close(true);
        }
      };

      document.addEventListener('keydown', this.closeListener.bind(this));
    },

    beforeDestroy()
    {
      document.removeEventListener('keydown', this.closeListener);
    },

    methods: {
      async search()
      {
        if (!this.query)
        {
          this.list.items = [];
          this.list.page = 1;
          this.list.totalPages = 1;
          return;
        }

        const result = await api.query(this.query, { pageSize: 20 });

        if (result.data)
        {
          this.list.items = result.data;
          this.list.page = result.paging.page;
          this.list.totalPages = result.paging.totalPages;
        }
      },

      onShortcutUpdated()
      {
        this.store.setShortcutEnabled(this.useShortcut);
      }
    }
  }
</script>

<style lang="scss">
  .app-search-overlay .app-overlay
  {
    padding: 0;
  }

  .app-search
  {
    padding: var(--padding-s);
  }

  .app-search-dialog
  {
  }

  .app-search-form
  {
    position: relative;
  }

  input.app-search-form-input
  {
    padding-right: 48px;
  }

  .app-search-submit
  {
    position: absolute;
    right: 0;
    height: 100%;
    width: 48px;
    justify-content: center;
  }

  .app-search-items
  {
    margin-top: var(--padding-s);
    font-size: var(--font-size);
    max-height: 490px;
    overflow-y: auto;
  }

  .app-search-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 20px 1fr auto;
    gap: 12px;
    align-items: center;
    position: relative;
    color: var(--color-text);
    padding: var(--padding-xs);
    border-radius: var(--radius);

    &:hover, &:focus
    {
      background: var(--color-tree-selected);
    }

    & + .app-search-item
    {
      margin-top: 2px;
    }
  }

  .app-search-item-text
  {
    display: flex;
    flex-direction: row;
    align-items: center;
    position: relative;
    top: 1px;
    font-size: var(--font-size);

    .-minor
    {
      font-size: var(--font-size-xs);
      color: var(--color-text-dim);
      margin-left: 0.5em;

      span
      {
        color: var(--color-text-dim-one);
      }
    }
  }

  .app-search-item-group
  {
    display: inline-flex;
    align-self: center;
    align-items: center;
    font-size: var(--font-size-xs);
    color: var(--color-text);
    font-weight: 600;
    background: var(--color-table-highlight);
    padding: 4px 12px;
    border-radius: 20px;
  }

  .app-search-item-icon
  {
    position: relative;
    color: var(--color-text);
  }

  .app-search-hint
  {
    display: flex;
    justify-content: space-between;
    margin-top: 0;
    border-bottom-left-radius: var(--radius);
    border-bottom-right-radius: var(--radius);
    background: var(--color-bg-shade-2);
    border-top: 1px solid var(--color-line-dashed);
    padding: var(--padding-s) var(--padding-s);
    font-size: var(--font-size-s);
    align-items: center;
    color: var(--color-text-dim);

    code
    {
      border: 1px dashed var(--color-line-dashed-onbg);
    }

    p + p
    {
      margin-top: 0;
    }
  }

  .app-search-shortcut
  {
    font-size: var(--font-size-s);
  }
</style>
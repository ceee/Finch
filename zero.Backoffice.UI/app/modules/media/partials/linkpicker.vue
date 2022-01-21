<template>
  <div v-if="area" class="ui-linkpicker-area-media">
    <ui-property :vertical="true">
      <ui-search v-model="search" @input="onSearch" />
    </ui-property>
    <ui-property>
      <div class="ui-list shop-linkpicker-area-media-items">
        <button v-if="id" type="button" class="-item" key="up" @click="onNavigate(parentId)">
          <span class="-icon">
            <ui-icon symbol="fth-corner-left-up" />
          </span>
          <p class="-content">..</p>
        </button>
        <button type="button" class="-item" v-for="item in items" :key="item.id" :class="{'is-selected': selection.indexOf(item.id) > -1}" @click="item.isFolder ? onNavigate(item.id) : onSelect(item.id)">
          <span class="-icon">
            <ui-thumbnail v-if="item.preview" :media="item.id" :alt="item.name" class="-image" />
            <ui-icon v-else :symbol="item.isFolder ? 'fth-folder' : 'fth-file'" />
          </span>
          <p class="-content ui-list-item-content">
            <span class="-text ui-list-item-text">{{item.name}}</span>
            <span v-if="item.isFolder" class="ui-list-item-description"  v-localize="{ key: item.children === 1 ? '@media.child_count_1' : '@media.child_count_x', tokens: { count: item.children }}"></span>
          </p>
          <ui-icon v-if="item.isFolder" symbol="fth-chevron-right" class="-aside" />
          <ui-icon v-if="selection.indexOf(item.id) > -1" symbol="fth-check-circle" class="-aside" />
        </button>
      </div>
      <ui-pagination :pages="paging.totalPages" :page="paging.page" @change="setPage" :inline="true" />
    </ui-property>
  </div>
</template>


<script>
  import api from '../api';
  import { debounce } from '../../../utils';

  export default {
    name: 'uiLinkpickerAreaMedia',

    props: {
      value: {
        type: Object,
        required: true
      },
      area: {
        type: Object,
        required: true
      }
    },

    data: () => ({
      selection: [],
      search: null,
      debouncedSearch: null,
      items: [],
      paging: {
        page: 1,
        pageSize: 10
      },
      parent: null,
      current: null,
      id: null
    }),

    watch: {
      value()
      {
        this.selection = [this.value.values.id];
      }
    },

    mounted()
    {
      this.debouncedSearch = debounce(() =>
      {
        this.paging.page = 1;
        this.load(this.id);
      }, 300);
      this.selection = this.value.values.id ? [this.value.values.id] : [];
      this.load(this.id);
    },

    methods: {

      isValid()
      {
        return this.selection.length > 0;
      },

      onSearch(value)
      {
        this.debouncedSearch();
      },

      async load(parentId)
      {
        if (this.search)
        {
          this.id = null;
        }

        let result = null;
        let query = {
          search: this.search,
          page: this.paging.page,
          pageSize: this.paging.pageSize
        };

        if (query.search)
        {
          result = await api.search(this.search, null, query);
        }
        else
        {
          result = await api.folders.getChildren((parentId || 'root'), true, query);
        }

        this.items = result.data.filter(x => x.id !== 'recyclebin');
        this.paging = result.paging;

        if (!query.search)
        {
          this.parentId = result.properties.parentId;
        }
        this.id = parentId;
      },

      setPage(page)
      {
        this.paging.page = page;
        this.load(this.id);
      },

      async onNavigate(id)
      {
        this.paging.page = 1;
        this.search = null;
        await this.load(id);
      },

      onSelect(id)
      {
        console.info('select: ' + id);
        if (id)
        {
          this.selection = [id];
          this.value.values = { id };
        }
        else
        {
          this.selection = [];
          this.value.values = { id: null };
        }
        this.$emit('change', this.value);
        this.$emit('input', this.value);
        this.$emit('update:value', this.value);
      },
    }
  }
</script>

<style lang="scss">
  .shop-linkpicker-area-media-items
  {
    .-item
    {
      display: grid;
      grid-template-columns: auto 1fr auto;
      grid-gap: 16px;
      align-items: center;
      width: 100%;

      & + .-item
      {
        margin-top: 10px;
      }

      &.is-selected .-text
      {
        font-weight: 700;
      }
    }

    .-icon
    {
      width: 48px;
      height: 48px;
      display: inline-flex;
      justify-content: center;
      align-items: center;
      border-radius: var(--radius-inner);
      background: var(--color-button-light);
      border: 1px solid transparent;
      color: var(--color-text);
      text-align: center;
      font-size: 16px;
      flex-shrink: 0;
      overflow: hidden;
    }

    .-image
    {
      width: 100%;
      height: 100%;
      object-fit: contain;
      background: var(--color-image-bg);
    }

    .-aside
    {

    }
  }
</style>
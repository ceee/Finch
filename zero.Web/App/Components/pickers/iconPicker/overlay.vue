<template>
  <ui-overlay-editor class="ui-iconpicker-overlay">
    <template v-slot:header>
      <ui-header-bar :title="config.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="white" :label="config.closeLabel" @click="config.hide"></ui-button>
    </template>

    <ui-search class="ui-iconpicker-overlay-search" v-model="query" />

    <div class="ui-iconpicker-overlay-colors">
      <i v-for="col in colors" :class="{ 'is-active': ('color-' + col) === color || (col === 'default' && !color), ['bg-color-' + col]: true }" @click="selectColor(col)" :title="color"></i>
    </div>

    <hr class="ui-iconpicker-overlay-line">

    <div class="ui-iconpicker-overlay-items" :class="color">
      <button v-for="item in items" type="button" class="ui-iconpicker-overlay-item" :class="{ 'is-active': item === icon, [color]: item === icon }" :title="item" @click="select(item)">
        <i :class="item"></i>
      </button>
    </div>
  </ui-overlay-editor>
</template>


<script>
  import { debounce as _debounce, filter as _filter } from 'underscore';

  export default {

    props: {
      model: String,
      config: Object
    },

    data: () => ({
      colors: [ 'default', 'gray', 'blue-gray', 'blue', 'teal', 'green', 'lime', 'yellow', 'orange', 'red', 'purple', 'brown' ],
      icon: null,
      color: null,
      query: '',
      items: []
    }),

    watch: {
      model()
      {
        this.init();
      },
      query()
      {
        this.debouncedSearch();
      }
    },

    created()
    {
      this.debouncedSearch = _debounce(this.search, 100);
      this.items = this.config.items;
      this.init();
    },

    methods: {

      confirm()
      {
        const result = (this.icon || '') + ' ' + (this.color || '').trim();
        this.config.confirm(result);
      },

      select(item)
      {
        this.icon = item;
        this.confirm();
      },

      selectColor(color)
      {
        this.color = color === 'default' ? null : 'color-' + color;
      },

      init()
      {
        if (!this.model)
        {
          this.icon = null;
          this.color = null;
        }
        else
        {
          const parts = this.model.split(' ');
          this.icon = parts[0];
          this.color = parts.length > 1 ? parts[1] : null;
        }
      },

      search()
      {
        const query = this.query;

        if (!query)
        {
          this.items = this.config.items;
        }
        else
        {
          this.items = _filter(this.config.items, function (item)
          {
            return item.toLowerCase().indexOf(query) > -1;
          })
        }
      }
    }
  }
</script>

<style lang="scss">
  .ui-iconpicker-overlay
  {

  }

  .ui-iconpicker-overlay-items
  {
    display: grid;
    grid-gap: 0;
    grid-template-columns: repeat(auto-fill, 61px);
    align-items: stretch;
  }

  .ui-iconpicker-overlay-item
  {
    display: block;
    text-align: center;
    font-size: 20px;
    height: 60px;
    border-radius: var(--radius);
    border: 2px solid transparent;

    &:hover, &.is-active
    {
      background: var(--color-box);
      box-shadow: 0 0 20px var(--color-shadow);
    }

    /*&.is-active
    {
      border-color: var(--color-line);
    }*/
  }

  .ui-iconpicker-overlay-search
  {
    margin-bottom: 20px;
  }

  .ui-iconpicker-overlay-colors
  {
    text-align: center;
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin: 0 2px;

    i
    {
      display: inline-block;
      width: 16px;
      height: 16px;
      border-radius: 20px;
      cursor: pointer;
      border: 2px solid transparent;
      transition: transform 0.2s ease;

      &.is-active
      {
        transform: scale(1.4);
      }
    }
  }

  .ui-iconpicker-overlay-line
  {

  }
</style>
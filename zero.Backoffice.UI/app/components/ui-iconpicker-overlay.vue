<template>
  <ui-trinity class="ui-iconpicker-overlay">
    <template v-slot:header>
      <ui-header-bar title="@iconpicker.title" :back-button="false" :close-button="true" @close="config.close(true)" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.close"></ui-button>
    </template>

    <ui-search class="ui-iconpicker-overlay-search onbg" v-model="query" />

    <div class="ui-iconpicker-overlay-colors" v-if="config.model.colors">
      <i v-for="col in colors" :class="{ 'is-active': ('color-' + col) === color || (col === 'default' && !color), ['bg-color-' + col]: true }" @click="selectColor(col)" :title="col"></i>
    </div>

    <!--<div class="ui-iconpicker-overlay-size">
      <input type="range" min="14" max="48" step="2" v-model.number="size" />
    </div>-->
    <!--<hr class="ui-iconpicker-overlay-line">-->

    <div class="ui-iconpicker-overlay-items" :style="{ 'grid-template-columns': 'repeat(' + columns + ', 1fr)' }">
      <button v-for="item in items" type="button" class="ui-iconpicker-overlay-item" :class="{ 'is-active': item === icon, [color]: item === icon }" :title="item" @click="select(item)">
        <ui-icon :symbol="item" :size="size" />
        <!--<i :class="item"></i>-->
      </button>
    </div>
  </ui-trinity>
</template>


<script>
  import { debounce } from '../utils';

  export default {

    props: {
      config: Object
    },

    data: () => ({
      value: null,
      file: null,
      colors: ['default', 'gray', 'blue-gray', 'blue', 'teal', 'green', 'lime', 'yellow', 'orange', 'red', 'purple', 'brown'],
      icon: null,
      color: null,
      query: '',
      set: null,
      items: [],
      size: 20
    }),

    watch: {
      value()
      {
        this.init();
      },
      query()
      {
        this.debouncedSearch();
      }
    },

    computed: {
      columns()
      {
        return ~~(580 / (this.size + 45));
      }
    },

    created()
    {
      this.debouncedSearch = debounce(this.search, 100);
      this.set = this.config.model.set;
      this.value = this.config.model.value;
      this.items = this.set.icons;
      this.init();
    },

    mounted()
    {
      let $selected = this.$el.querySelector('.ui-iconpicker-overlay-item.is-active');

      if ($selected)
      {
        let $scrollable = this.$el.querySelector('content');
        const offset = $selected.offsetTop - $scrollable.clientHeight * 0.5 - 30;
        $scrollable.scrollTop = offset < 0 ? 0 : offset;
      }
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
        if (!this.value)
        {
          this.icon = null;
          this.color = null;
        }
        else
        {
          const parts = this.value.split(' ');
          this.icon = parts[0];
          this.color = parts.length > 1 ? parts[1] : null;
        }
      },

      search()
      {
        const query = this.query;

        if (!query)
        {
          this.items = this.set.icons;
        }
        else
        {
          this.items = this.set.icons.filter(item => item.toLowerCase().indexOf(query) > -1);
        }
      }
    }
  }
</script>

<style lang="scss">
  .ui-iconpicker-overlay content
  {
    padding-top: 0;
  }

  .ui-iconpicker-overlay-items
  {
    display: grid;
    grid-template-columns: repeat(auto-fill, 61px);
    grid-gap: 8px;
    align-items: stretch;
  }

  .ui-iconpicker-overlay-item
  {
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 22px 0;
    border-radius: var(--radius);

    &:hover
    {
      background: var(--color-box);
      box-shadow: var(--shadow-short);
    }

    &.is-active
    {
      background: var(--color-primary);
      color: var(--color-primary-text);
      box-shadow: var(--shadow-short);
    }

    &.is-active
    {
      //color: var(--color-primary);
    }
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
    margin: 0 20px;

    i
    {
      display: inline-block;
      width: 16px;
      height: 16px;
      border-radius: 20px;
      cursor: pointer;
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
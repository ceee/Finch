<template>
  <div class="ui-pick" :class="{'is-disabled': disabled }">
    
    <!-- previews -->
    <div class="ui-pick-previews" v-if="previews.length > 0">
      <div v-if="configuration.preview.enabled && !configuration.preview.combined" v-for="preview in previews" class="ui-pick-preview">
        <ui-select-button :icon="getPreviewIcon(preview)" :label="preview.name" :description="getPreviewDescription(preview)" :disabled="disabled" @click="pick(preview.id)" :tokens="preview" />
        <ui-icon-button v-if="!disabled && configuration.preview.delete" @click="remove(preview.id)" icon="fth-x" title="@ui.close" />
      </div>
    </div>

    <!-- add button -->
    <ui-select-button v-if="canAdd && configuration.addButton.enabled" icon="fth-plus" :label="configuration.addButton.label" @click="pick()" :disabled="disabled" />

    <!-- overlay -->
    <ui-dropdown ref="overlay" class="ui-pick-overlay">
      <div class="ui-pick-overlay-head">
        <div class="ui-pick-overlay-head-title">
          <span class="-name" v-localize="title"></span>
          <span class="-max" v-if="configuration.limit > 1" v-localize="{ key: '@ui.pick.max', tokens: { count: configuration.limit }}"></span>
        </div>
        <ui-icon-button @click="hide" icon="fth-x" title="@ui.close" />
      </div>
      <ui-search class="ui-pick-overlay-search" />

      <!-- empty results -->
      <div class="ui-pick-overlay-items">
        <button v-for="item in items" :key="item.id" type="button" class="ui-pick-overlay-item">
          <i v-if="item.icon" class="-icon" :class="item.icon" />
          <span class="-name" v-localize="item.name"></span>
          <span v-if="item.text && configuration.list.description" class="-text" v-localize="item.text"></span>
        </button>
      </div>

      <!-- loading + empty states -->
      <div class="ui-pick-overlay-center">
        <div v-if="!isLoading && !configuration.autocomplete && !items.length" class="ui-pick-overlay-message">
          <i class="ui-pick-overlay-message-icon fth-list"></i>
          No items found
        </div>
        <ui-loading v-if="isLoading" />
      </div>
    </ui-dropdown>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay';
  import { extend as _extend, filter as _filter } from 'underscore';

  const defaultConfig = {
    // picker items, can either be a static list or a promise
    items: [],
    // exclude Ids from the picker selection items
    excludedIds: [],
    // autocomplete allows entering custom texts
    autocomplete: false,  
    // maximum selection count
    limit: 10,
    // whether the previews/results are sortable or not
    sortable: true,
    // close picker when an item is clicked
    closeOnClick: true,
    // title in dropdown
    title: null,

    addButton: {
      // hide the add button
      enabled: true,
      // text key for the add button
      label: '@ui.select'
    },

    list: {
      // output description text if available (second line)
      description: false
    },

    preview: {
      // output previews
      enabled: true,
      // output all selected items in one line (comma-separated)
      combined: false,
      // prefixed title when combine=true
      combinedTitle: null,
      // default icon used for previews
      defaultIcon: 'fth-square',
      // hides the icon in the preview
      icon: true,
      // hides the delete icon in preview
      delete: true,
      // displays an image instead of an icon in the preview
      iconAsImage: false,
      // output description text if available (second line)
      description: false
    },

    search: {
      // hides the search input
      enabled: true,
      // can force a local search for remote items (via promise)
      local: true,
      // sets the current model as the search input when opened
      setDefaultValue: false,
      // focus search input on open
      focus: false,
      // placeholder key in search input
      placeholder: null //vm.options.autocomplete ? 'ui_search_autocomplete' : 'ui_search',
    }
  };

  export default {
    name: 'uiPick',

    props: {
      value: {
        type: [String, Array],
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: {
        type: Object,
        default: () =>
        {
          return defaultConfig;
        }
      }
    },


    watch: {
      config()
      {
        this.buildConfig();
      },
      value(val)
      {
        if (val)
        {
          this.load();
        }
      }
    },


    computed: {
      canAdd()
      {
        return (!this.value && this.limit < 2) || (!this.value || this.value.length < this.configuration.limit);
      },
      isRemote()
      {
        return typeof this.configuration.items === 'function';
      },
      title()
      {
        if (this.configuration.title) return this.configuration.title;
        if (this.configuration.autocomplete) return '@ui.pick.title_autocomplete';
        if (this.configuration.limit > 1) return '@ui.pick.title_multiple';
        return '@ui.pick.title';
      }
    },


    data: () => ({
      configuration: {},
      previews: [],
      allItems: [],
      items: [],
      loaded: false,
      isLoading: false
    }),


    created()
    {
      this.buildConfig();
    },


    methods: {

      buildConfig()
      {
        var config = JSON.parse(JSON.stringify(defaultConfig));
        this.configuration = _extend(defaultConfig, this.config);
        this.configuration.search = _extend(config.search, this.config.search || {});
        this.configuration.addButton = _extend(config.addButton, this.config.addButton || {});
        this.configuration.preview = _extend(config.preview, this.config.preview || {});
        this.configuration.list = _extend(config.preview, this.config.list || {});
      },


      pick()
      {
        this.$refs.overlay.toggle();

        if (!this.loaded)
        {
          this.load();
        }
      },


      hide()
      {
        this.$refs.overlay.hide();
      },


      remove()
      {

      },


      // loads remote items or items from the given configuration
      // this will store both all items and the reduced search results
      load()
      {
        if (this.isLoading)
        {
          return;
        }

        this.isLoading = true;
        this.allItems = [];

        let onLoaded = (items) =>
        {
          this.allItems = items;
          this.loadSuggestions();
          this.loaded = true;
          this.isLoading = false;
        };

        if (this.isRemote)
        {
          this.configuration.items().then(onLoaded);
        }
        else
        {
          onLoaded(this.configuration.items);
        }
      },


      loadSuggestions()
      {
        this.isLoading = true;

        let items = [];
        let search = null; // TODO use search value vm.searchTerm.toLowerCase();

        if (!search)
        {
          items = this.allItems;
        }
        else
        {
          if (!this.isRemote || this.configuration.search.local)
          {
            items = _.filter(this.allItems, (item) => item.name.toLowerCase().indexOf(search) > -1);
          }
          else if (this.isRemote)
          {
            configuration.items(search).then(res => this.items = res);
          }
        }

        if (this.configuration.excludedIds && this.configuration.excludedIds.length)
        {
          items = _.filter(items, (item) => this.configuration.excludedIds.indexOf(item.id) < 0);
        }

        this.items = items;
        this.isLoading = false;
      },


      getPreviewIcon(preview)
      {
        return this.configuration.preview.icon ? (preview.icon || this.configuration.preview.defaultIcon) : null;
      },


      getPreviewDescription(preview)
      {
        return this.configuration.preview.description ? preview.text : null;
      }
    }
  }
</script>

<style lang="scss">
  .ui-pick-overlay-search
  {
    margin: 15px 15px 5px;

    .ui-input
    {
      background: var(--color-highlight);
      min-width: 0;
    }
  }

  .ui-pick-overlay 
  {
    .ui-dropdown
    {
      min-width: 0;
      width: 100%;
      max-width: 380px;
    }
  }

  .ui-pick-overlay-center
  {
    display: flex;
    justify-content: center;
    margin: 30px 0;
  }

  .ui-pick-overlay-head
  {
    padding: 15px 15px 5px;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .ui-pick-overlay-head-title
  {
    font-size: var(--font-size-l);
    font-weight: 600;

    .-max
    {
      font-size: var(--font-size-s);
      font-weight: 400;
      color: var(--color-fg-light);
      margin-left: .6em;
    }
  }

  .ui-pick-overlay-message
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    text-align: center;
    padding: 0 15px;
  }

  .ui-pick-overlay-message-icon
  {
    font-size: 18px;
    margin-bottom: 10px;
  }

  .ui-pick-overlay-items
  {

  }

  .ui-pick-overlay-item
  {
    display: flex;
    padding: 5px 15px;
  }
</style>
<template>
  <div class="ui-pick" :class="{'is-disabled': disabled, 'is-combined': configuration.preview.combined }">
    
    <!-- previews -->
    <div class="ui-pick-previews" v-if="configuration.preview.enabled && previews.length > 0 && !configuration.preview.combined">
      <div v-for="preview in previews" class="ui-pick-preview">
        <ui-select-button :icon="getPreviewIcon(preview)" :icon-as-image="configuration.preview.iconAsImage" :label="preview[configuration.keys.name]" :description="getPreviewDescription(preview)" :disabled="disabled" @click="pick(preview.id)" :tokens="preview" />
        <ui-icon-button v-if="!disabled && configuration.preview.delete" @click="remove(preview.id)" icon="fth-x" title="@ui.close" />
      </div>
    </div>

    <!-- combined preview -->
    <div class="ui-pick-previews" v-if="configuration.preview.enabled && configuration.preview.combined">
      <div class="ui-pick-preview">
         <ui-select-button icon="fth-check-circle" :label="combinedTitle" :disabled="disabled" @click="pick()" />
      </div>
    </div>

    <!-- add button -->
    <div class="ui-pick-add" v-if="canAdd && configuration.addButton.enabled && !configuration.preview.combined">
      <slot name="add">
        <ui-select-button icon="fth-plus" :label="configuration.addButton.label" @click="pick()" :disabled="disabled" />
      </slot>
    </div>    

    <!-- overlay -->
    <ui-dropdown ref="overlay" class="ui-pick-overlay" @opened="overlayOpened">

      <!-- headline -->
      <div class="ui-pick-overlay-head">
        <div class="ui-pick-overlay-head-title">
          <span class="-name" v-localize="title"></span>
          <span class="-max" v-if="configuration.limit > 1" v-localize="{ key: '@ui.pick.max', tokens: { count: configuration.limit }}"></span>
        </div>
        <ui-icon-button @click="hide" icon="fth-x" title="@ui.close" />
      </div>

      <!-- search -->
      <ui-search v-if="configuration.search.enabled" ref="search" class="ui-pick-overlay-search" :value="searchValue" @input="onSearch" @submit="onSearchSubmit">
        <template v-slot:button="search" v-if="configuration.autocomplete">
          <button type="button" class="ui-searchinput-button" v-localize:title="'@ui.search.button'" @click="search.onSubmit"><i class="fth-check"></i></button>
        </template>
      </ui-search>

      <!-- items -->
      <div class="ui-pick-overlay-items">
        <button v-for="item in items" :key="item.id" type="button" class="ui-pick-overlay-item" @click="select(item)" :class="{'is-selected': isSelected(item) }">
          <i v-if="item[configuration.keys.icon]" class="-icon" :class="item[configuration.keys.icon]" />
          <div class="ui-pick-overlay-item-title">
            <span class="-name" v-localize="item[configuration.keys.name]"></span>
            <span v-if="item[configuration.keys.description] && configuration.list.description" class="-text" v-localize="item[configuration.keys.description]"></span>
          </div>
        </button>
      </div>

      <!-- loading + empty states -->
      <div class="ui-pick-overlay-center" v-if="isLoading || (!configuration.autocomplete && !items.length)">
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
  import { extend as _extend, filter as _filter, debounce as _debounce, isArray as _isArray, clone as _clone, find as _find, map as _map } from 'underscore';

  const defaultConfig = {
    // picker items, can either be a static list or a promise
    items: [],
    // preview items, can either be a static list or a promise. If null, take the items list
    previews: [],
    // exclude Ids from the picker selection items
    excludedIds: [],
    // autocomplete allows entering custom texts
    autocomplete: false,  
    // maximum selection count
    limit: 10,
    // multiple selection
    multiple: true,
    // whether the previews/results are sortable or not
    sortable: true,
    // close picker when an item is clicked
    closeOnClick: true,
    // title in dropdown
    title: null,
    // automatically open picker
    autoOpen: false,

    keys: {
      // name key
      name: 'name',
      // description key
      description: 'text',
      // icon key
      icon: 'icon'
    },

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
      description: true
    },

    search: {
      // hides the search input
      enabled: true,
      // can force a local search for remote items (via promise)
      local: true,
      // sets the current model as the search input when opened
      setDefaultValue: false,
      // focus search input on open
      focus: true,
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
      config: {
        deep: true,
        handler(newval, oldval)
        {
          if (JSON.stringify(newval) !== JSON.stringify(oldval))
          {
            this.buildConfig();
            this.loaded = false;          
          }
        }
      },
      value(val)
      {
        this.onValueChanged(val);
      }
    },


    computed: {
      multiple()
      {
        return this.configuration.multiple;
      },
      canAdd()
      {
        return (!this.value && !this.multiple) || (!this.value || this.value.length < this.configuration.limit);
      },
      isRemote()
      {
        return typeof this.configuration.items === 'function';
      },
      title()
      {
        if (!!this.configuration.title) return this.configuration.title;
        if (this.configuration.autocomplete) return '@ui.pick.title_autocomplete';
        if (this.multiple) return '@ui.pick.title_multiple';
        return '@ui.pick.title';
      },
      combinedTitle()
      {
        let html = this.configuration.preview.combinedTitle ? this.configuration.preview.combinedTitle : '';

        if (this.previews.length > 0)
        {
          html += ': <b>' + _map(this.previews, p => p[this.configuration.keys.name]).join(', ') + '</b>';
        }

        return html;
      }
    },


    data: () => ({
      configuration: {},
      previews: [],
      allItems: [],
      items: [],
      selected: [],
      loaded: false,
      isLoading: false,
      debouncedUpdate: null,
      searchValue: ''
    }),


    created()
    {
      this.buildConfig();
      this.debouncedUpdate = _debounce(this.loadSuggestions, 300);
      this.onValueChanged(this.value);
    },


    mounted()
    {
      if (this.configuration.autoOpen)
      {
        this.pick();
      }
    },


    methods: {

      refresh()
      {
        this.buildConfig();
        this.loaded = false;
      },

      onValueChanged(val)
      {
        if (this.multiple)
        {
          this.selected = _isArray(val) && val.length ? _clone(val) : [];
        }
        else
        {
          this.selected = val ? [val] : [];
        }

        this.loadPreviews();
      },


      buildConfig()
      {
        var config = JSON.parse(JSON.stringify(defaultConfig));
        this.configuration = _extend(JSON.parse(JSON.stringify(config)), this.config);
        this.configuration.search = _extend(config.search, this.config.search || {});
        this.configuration.addButton = _extend(config.addButton, this.config.addButton || {});
        this.configuration.preview = _extend(config.preview, this.config.preview || {});
        this.configuration.list = _extend(config.list, this.config.list || {});
        this.configuration.keys = _extend(config.keys, this.config.keys || {});
      },


      overlayOpened()
      {
        if (!this.loaded)
        {
          this.load();
        }
        if (this.configuration.search.enabled && this.configuration.search.focus)
        {
          this.$nextTick(() => this.$refs.search.focus());
        }
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


      remove(id)
      {
        let index = this.selected.indexOf(id);
        this.selected.splice(index, 1);
        this.onChange(this.multiple ? this.selected : null);
      },


      clear()
      {
        this.selected = [];
        this.onChange(this.multiple ? this.selected : null);
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
        let items = [];
        let search = this.searchValue;

        let handleResult = (res) =>
        {
          if (this.configuration.excludedIds && this.configuration.excludedIds.length)
          {
            res = _filter(res, (item) => this.configuration.excludedIds.indexOf(item.id) < 0);
          }

          this.items = res;
        };

        if (!search)
        {
          items = this.allItems;
        }
        else
        {
          if (!this.isRemote || this.configuration.search.local)
          {
            items = _filter(this.allItems, (item) => item.name.toLowerCase().indexOf(search) > -1);
          }
          else if (this.isRemote)
          {
            this.configuration.items(search).then(res => handleResult(res));
            return;
          }
        }

        handleResult(items);
      },


      loadPreviews()
      {
        let onLoaded = (items, needsFilter) =>
        {
          if (needsFilter)
          {
            this.previews = [];
            this.selected.forEach(id =>
            {
              let res = _find(items, item => item.id === id);

              if (!res)
              {
                // TODO push error output
              }
              else
              {
                this.previews.push(_clone(res));
              }
            });
          }
          else
          {
            this.previews = items;
          }

          //console.info(JSON.parse(JSON.stringify(this.previews)));
        };

        if (this.configuration.autocomplete)
        {
          onLoaded(_map(this.selected, s =>
          {
            let item = { id: s };
            item[this.configuration.keys.name] = s;
            return item;
          }), false);
        }
        else
        {
          let promise = (_isArray(this.configuration.previews) && this.configuration.previews.length) || typeof this.configuration.previews === 'function' ? this.configuration.previews : this.configuration.items;
          let isFunc = typeof promise === 'function';

          if (isFunc && this.selected.length > 0)
          {
            promise(this.selected).then(onLoaded);
          }
          else if (isFunc)
          {
            onLoaded([]);
          }
          else
          {
            onLoaded(promise, true);
          }
        }
      },


      onSearch(value)
      {
        this.searchValue = value;
        if (!this.isRemote || this.configuration.search.local)
        {
          this.loadSuggestions();
        }
        else
        {
          this.debouncedUpdate();
        }
      },


      onSearchSubmit()
      {
        let value = this.searchValue.trim();
        let item = {};
        item.id = value;
        item[this.configuration.keys.name] = value;
        this.select(item);
      },


      select(item)
      {
        let value = this.configuration.autocomplete ? item[this.configuration.keys.name] : item.id;

        if (this.multiple)
        {
          if (!this.canAdd)
          {
            if (this.limit > 1)
            {
              return;
            }

            this.selected = [];
          }

          var index = this.selected.indexOf(value);

          if (index > -1)
          {
            this.selected.splice(index, 1);
            this.onDeselected(value, index);
          }
          else
          {
            this.selected.push(value);
            this.onSelected(value, item);
          }

          this.onChange(this.selected);
        }
        else
        {
          this.selected = [value];
          this.onChange(value);
          this.onSelected(value, item);
        }
      },


      onSelected(value, item)
      {
        this.$emit('select', value, item);
      },


      onDeselected(value, index)
      {
        this.$emit('deselect', value, index);
      },


      onChange(value)
      {
        this.$emit('input', value);
        this.$emit('change', value);

        if (this.configuration.closeOnClick)
        {
          this.$refs.overlay.hide();
        }
      },


      isSelected(item)
      {
        let value = this.configuration.autocomplete ? item[this.configuration.keys.name] : item.id;
        return this.selected.indexOf(value) > -1;
      },


      getPreviewIcon(preview)
      {
        return this.configuration.preview.icon ? (preview[this.configuration.keys.icon] || this.configuration.preview.defaultIcon) : null;
      },


      getPreviewDescription(preview)
      {
        return this.configuration.preview.description ? preview[this.configuration.keys.description] : null;
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

    .ui-icon-button
    {
      background: none;
    }
  }

  .ui-pick-overlay-head-title
  {
    font-size: var(--font-size-l);
    font-weight: 600;

    .-max
    {
      font-size: var(--font-size-s);
      font-weight: 400;
      color: var(--color-text-dim);
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
    margin: 15px 0;
    max-height: 290px;
    overflow-y: auto;
  }

  .ui-pick-overlay-item
  {
    display: grid;
    grid-template-columns: auto 1fr;
    padding: 10px 18px;
    min-height: 42px;
    width: 100%;
    border-radius: var(--radius);
    align-items: center;
    transition: background .2s, transform .2s, opacity .2s;
    position: relative;

    &:hover
    {
      background: var(--color-tree-selected);
    }

    &.is-selected
    {
      padding-right: 32px;
      
      .-name
      {
        font-weight: 600;
      }
    }

    &.is-selected:after
    {
      font-family: "Feather";
      content: "\e83e";
      font-size: 16px;
      color: var(--color-primary);
      position: absolute;
      right: 20px;
    }

    .-icon
    {
      font-size: var(--font-size-l);
      margin-right: var(--padding-s);
    }
  }

  .ui-pick-overlay-item-title
  {
    .-name
    {
      display: block;
    }

    .-text
    {
      display: block;
      color: var(--color-text-dim);
      font-size: var(--font-size-s);
      margin-top: 3px;
    }
  }

  .ui-pick-preview
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .ui-pick-preview + .ui-pick-preview,
  .ui-pick-previews + .ui-select-button
  {
    margin-top: 10px;
  }

  .ui-pick.is-combined .ui-select-button-label
  {
    font-weight: 400;
  }

  .ui-pick-previews
  {
    .ui-icon-button 
    {
      height: 24px;
      width: 24px;

      .ui-button-icon
      {
        font-size: 13px;
      }
    }
  }
</style>
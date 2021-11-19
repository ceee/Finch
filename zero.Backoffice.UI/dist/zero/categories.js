import {n as normalizeComponent, s as CategoriesApi, N as Notification, A as Arrays, h as hub, t as ChannelsApi, O as Overlay} from "./index.js";
import {h as debounce, c as find} from "./vendor.js";
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "shop-categorycreate", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.category.add_category", expression: "'@shop.category.add_category'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "shop-categorycreate-items"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.item.name, expression: "item.name"}, {name: "localize", rawName: "v-localize:placeholder", value: "@shop.category.fields.name_placeholder", expression: "'@shop.category.fields.name_placeholder'", arg: "placeholder"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "200", readonly: _vm.disabled}, domProps: {value: _vm.item.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.item, "name", $event.target.value);
    }}})]), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: "@ui.create"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}})], 1)];
  }}], null, false, 703153608)}) : _vm._e();
};
var staticRenderFns$3 = [];
var create_vue_vue_type_style_index_0_lang = ".shop-categorycreate {\n  text-align: left;\n}\n.shop-categorycreate-items {\n  margin-top: var(--padding);\n}\n.shop-categorycreate-items .ui-property + .ui-property,\n.shop-categorycreate-items .ui-split + .ui-property {\n  margin-top: 0;\n}";
const script$3 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    item: {},
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(CategoriesApi.getEmpty()).then((response) => {
        this.disabled = false;
        //!response.canEdit;
        this.item = response.entity;
        this.item.parentId = this.model.parentId;
        this.item.channelId = this.model.channelId;
        if (this.item.channelId === zero.sharedAppId) {
          this.item.appId = zero.sharedAppId;
          this.item.channelId = null;
        } else if (this.item.channelId === this.zero.commerce.alias.localChannel) {
          this.item.channelId = null;
        }
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(CategoriesApi.save(this.item)).then((response) => {
        this.config.confirm(response.model, this.config);
      });
    }
  }
};
const __cssModules$3 = {};
var component$3 = normalizeComponent(script$3, render$3, staticRenderFns$3, false, injectStyles$3, null, null, null);
function injectStyles$3(context) {
  for (let o in __cssModules$3) {
    this[o] = __cssModules$3[o];
  }
}
component$3.options.__file = "../zero.Commerce/Plugin/pages/categories/overlays/create.vue";
var AddCategoryOverlay = component$3.exports;
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-categories-move", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.move.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.move.action", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: "@ui.move.text", tokens: {name: _vm.model.name}}, expression: "{ key: '@ui.move.text', tokens: { name: model.name } }", arg: "html"}], staticClass: "shop-categories-move-text"}), _c("div", {staticClass: "ui-box shop-categories-move-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems}, on: {select: _vm.onSelect}})], 1)]);
};
var staticRenderFns$2 = [];
var move_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.shop-categories-move .ui-box {\n  margin: 0;\n  padding: 16px 0;\n}\n.shop-categories-move .ui-box .ui-tree-item.is-disabled {\n  opacity: 0.5;\n}\n.shop-categories-move .ui-box .ui-tree-item.is-selected, .shop-categories-move .ui-box .ui-tree-item:hover:not(.is-disabled) {\n  background: var(--color-tree-selected);\n}\n.shop-categories-move .ui-box .ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n}\n.shop-categories-move .ui-box .ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}\n.shop-categories-move content {\n  padding-top: 0;\n}\n.shop-categories-move-text {\n  margin: 0 0 20px;\n}';
const script$2 = {
  props: {
    isCopy: {
      type: Boolean,
      default: false
    },
    model: Object,
    config: Object
  },
  data: () => ({
    items: [],
    selected: null,
    state: "default",
    cache: {},
    prevItem: null,
    selected: null
  }),
  mounted() {
    this.selected = this.model;
  },
  methods: {
    onSelect(item) {
      item.isSelected = true;
      if (this.prevItem && this.prevItem.id != item.id) {
        this.prevItem.isSelected = false;
      }
      this.prevItem = item;
      this.selected = item;
    },
    getItems(parent) {
      const key = !parent ? "__root" : parent;
      if (this.cache[key]) {
        return Promise.resolve(this.cache[key]);
      }
      return CategoriesApi.getChildren(this.config.channelId, parent, this.model.parentId).then((response) => {
        let items = response.items;
        if (!parent) {
          items.splice(0, 0, {
            id: null,
            parentId: null,
            sort: 0,
            name: "@page.root",
            icon: "fth-arrow-down-circle",
            isOpen: false,
            modifier: null,
            hasChildren: false,
            childCount: 0,
            isInactive: false,
            hasActions: false
          });
        }
        items.forEach((item) => {
          item.isSelected = this.model.parentId == item.id;
          if (item.isSelected) {
            this.prevItem = item;
          }
          item.disabled = item.id === "recyclebin" || item.id == this.model.id;
          item.hasActions = false;
        });
        this.cache[key] = items;
        return items;
      });
    },
    onSave() {
      if (this.model.parentId == this.selected.id) {
        this.config.close();
        return;
      }
      this.state = "loading";
      CategoriesApi.move(this.model.id, this.selected.id).then((res) => {
        if (res.success) {
          this.state = "success";
          this.config.confirm(res.model);
        } else {
          this.state = "error";
          Notification.error(res.errors[0].message);
        }
      });
    }
  }
};
const __cssModules$2 = {};
var component$2 = normalizeComponent(script$2, render$2, staticRenderFns$2, false, injectStyles$2, null, null, null);
function injectStyles$2(context) {
  for (let o in __cssModules$2) {
    this[o] = __cssModules$2[o];
  }
}
component$2.options.__file = "../zero.Commerce/Plugin/pages/categories/overlays/move.vue";
var MoveOverlay = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-categories-sort", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.sort.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.save", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize", value: "@ui.sort.text", expression: "'@ui.sort.text'"}], staticClass: "shop-categories-sort-text"}), _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {handle: ".is-handle", onUpdate: _vm.onSortingUpdated}, expression: "{ handle: '.is-handle', onUpdate: onSortingUpdated }"}], staticClass: "shop-categories-sort-items"}, _vm._l(_vm.items, function(item, index) {
    return _c("div", {key: item.id, staticClass: "shop-categories-sort-item"}, [_c("span", [_vm._v(_vm._s(index + 1) + ". "), _c("i", {staticClass: "shop-categories-sort-item-icon", class: item.icon}), _vm._v(" " + _vm._s(item.name))]), _c("button", {staticClass: "shop-categories-sort-item-button is-handle", attrs: {type: "button"}}, [_c("ui-icon", {staticClass: "-minor", attrs: {symbol: "fth-more-vertical", size: 14}})], 1)]);
  }), 0)]);
};
var staticRenderFns$1 = [];
var sort_vue_vue_type_style_index_0_lang = ".shop-categories-sort .ui-box {\n  margin: 0;\n}\n.shop-categories-sort content {\n  padding-top: 0;\n}\n.shop-categories-sort-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 1fr auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  height: 46px;\n  color: var(--color-text);\n  position: relative;\n  padding: 0 8px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n}\n.shop-categories-sort-item i {\n  font-size: var(--font-size-l);\n  position: relative;\n  top: -1px;\n  color: var(--color-text-dim);\n}\n.shop-categories-sort-item span {\n  padding: 12px 8px;\n}\n.shop-categories-sort-item.is-selected {\n  color: var(--color-text-dim);\n}\n.shop-categories-sort-item + .shop-categories-sort-item {\n  margin-top: 8px;\n}\nbutton.shop-categories-sort-item-button {\n  height: 48px;\n  width: 24px;\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  text-align: center;\n}\nbutton.shop-categories-sort-item-button i {\n  font-size: var(--font-size);\n}\nbutton.shop-categories-sort-item-button.is-handle {\n  cursor: move;\n}\n.shop-categories-sort-text {\n  margin: 0 0 20px;\n}\ni.shop-categories-sort-item-icon {\n  top: 0;\n  color: var(--color-text);\n  margin-right: 8px;\n}";
const script$1 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    items: [],
    selected: [],
    state: "default"
  }),
  computed: {
    parentId() {
      return this.model ? this.model.parentId : null;
    }
  },
  mounted() {
    return CategoriesApi.getChildren(this.config.channelId, this.parentId).then((response) => {
      this.items = response.items;
    });
  },
  methods: {
    onSave() {
      this.state = "loading";
      CategoriesApi.saveSorting(this.items.map((x) => x.id)).then((res) => {
        if (res.success) {
          this.state = "success";
          this.config.confirm(res.model);
        } else {
          this.state = "error";
          Notification.error(res.errors[0].message);
        }
      });
    },
    onSortingUpdated(ev) {
      this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
    }
  }
};
const __cssModules$1 = {};
var component$1 = normalizeComponent(script$1, render$1, staticRenderFns$1, false, injectStyles$1, null, null, null);
function injectStyles$1(context) {
  for (let o in __cssModules$1) {
    this[o] = __cssModules$1[o];
  }
}
component$1.options.__file = "../zero.Commerce/Plugin/pages/categories/overlays/sort.vue";
var SortOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.loaded ? _c("div", {staticClass: "shop-catalogue"}, [_c("div", {directives: [{name: "resizable", rawName: "v-resizable", value: _vm.resizable, expression: "resizable"}], staticClass: "app-tree shop-catalogue-tree"}, [_c("ui-header-bar", {staticClass: "ui-tree-header", attrs: {title: "@shop.catalogue.categories", "back-button": false}}, [_c("ui-dot-button", {on: {click: function($event) {
    return _vm.$refs.tree.onActionsClicked(null, $event);
  }}})], 1), _c("ui-dropdown", {staticClass: "shop-catalogue-tree-switch", scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light block", label: _vm.channelPreview, caret: "down"}})];
  }, proxy: true}], null, false, 3276616934)}, [_c("ui-search", {staticClass: "shop-catalogue-tree-switch-search", model: {value: _vm.channelSearch, callback: function($$v) {
    _vm.channelSearch = $$v;
  }, expression: "channelSearch"}}), !_vm.channelSearch ? _c("ui-dropdown-separator") : _vm._e(), _vm._l(_vm.catalogues, function(catalogue) {
    return !_vm.channelSearch && !catalogue.hidden ? _c("ui-dropdown-button", {key: catalogue.id, attrs: {icon: catalogue.icon, value: catalogue, label: catalogue.name, selected: _vm.channelId == catalogue.id}, on: {click: _vm.onChannelChange}}) : _vm._e();
  }), _c("ui-dropdown-separator"), _c("div", {staticClass: "shop-catalogue-tree-switch-items"}, _vm._l(_vm.channels, function(channel) {
    return _c("ui-dropdown-button", {key: channel.id, attrs: {value: channel, label: channel.name, selected: _vm.channelId == channel.id}, on: {click: _vm.onChannelChange}});
  }), 1)], 2), _c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, config: _vm.treeConfig, active: _vm.id}, scopedSlots: _vm._u([{key: "actions", fn: function(props) {
    return [props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.edit.title", icon: "fth-edit-2"}, on: {click: function($event) {
      return _vm.$router.push({name: "commerce-categories-edit", params: {id: props.item.id}});
    }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.create", icon: "fth-plus"}, on: {click: function($event) {
      return _vm.create(props.item != null ? props.item.id : null);
    }}}), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
      return _vm.move(props.item);
    }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.sort.title", icon: "fth-arrow-down"}, on: {click: function($event) {
      return _vm.sort(props.item);
    }}}), props.item ? _c("ui-dropdown-separator") : _vm._e(), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash"}, on: {click: function($event) {
      return _vm.remove(props.item);
    }}}) : _vm._e()];
  }}, {key: "bottom", fn: function() {
    return [_c("ui-tree-item", {attrs: {value: _vm.addCategoryTreeItem}, on: {click: function($event) {
      return _vm.create(null);
    }}})];
  }, proxy: true}], null, false, 4098363710)}, [_vm.selected && _vm.selected.id === _vm.catalogues[0].id ? _c("div", {directives: [{name: "localize", rawName: "v-localize:html", value: "@shop.catalogue.sharedCatalogueInfo", expression: "'@shop.catalogue.sharedCatalogueInfo'", arg: "html"}], staticClass: "shop-catalogue-tree-hint"}) : _vm._e(), _vm.selected && _vm.selected.id === _vm.catalogues[1].id ? _c("div", {directives: [{name: "localize", rawName: "v-localize:html", value: "@shop.catalogue.localCatalogueInfo", expression: "'@shop.catalogue.localCatalogueInfo'", arg: "html"}], staticClass: "shop-catalogue-tree-hint"}) : _vm._e()]), _c("div", {staticClass: "shop-catalogue-tree-resizable ui-resizable"})], 1), _c("main", {staticClass: "shop-catalogue-main"}, [_c("router-view")], 1)]) : _vm._e();
};
var staticRenderFns = [];
var categories_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.shop-catalogue {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 2px;\n  justify-content: stretch;\n  height: 100vh;\n}\n.shop-catalogue-main {\n  max-height: 100%;\n  font-size: var(--font-size);\n  overflow-y: auto;\n}\n.app-tree.shop-catalogue-tree {\n  overflow: visible;\n  display: flex;\n  flex-direction: column;\n}\n.app-tree.shop-catalogue-tree .ui-header-bar {\n  margin-bottom: 0;\n  flex-shrink: 0;\n}\n.app-tree.shop-catalogue-tree .ui-tree {\n  overflow-y: auto;\n}\n.shop-catalogue-tree-switch {\n  padding: 0 var(--padding) 0;\n  margin-bottom: 30px;\n  flex-shrink: 0;\n  /*.ui-dropdown-button-icon\n    {\n      color: var(--color-text);\n    }*/\n}\n.shop-catalogue-tree-switch .-minor, .shop-catalogue-tree-switch .ui-button-text {\n  font-weight: 400;\n}\n.shop-catalogue-tree-switch .ui-dropdown {\n  min-width: 320px;\n  max-width: 360px;\n}\n.shop-catalogue-tree-switch .ui-dropdown-toggle .ui-button-text {\n  display: block;\n  overflow: hidden;\n  white-space: nowrap;\n  text-overflow: ellipsis;\n}\n.shop-catalogue-tree-switch-items {\n  max-height: 340px;\n  overflow-y: scroll;\n}\n.shop-catalogue-overview {\n  padding: 95px 0 0 60px;\n}\n.shop-catalogue-overview-action {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 35px;\n  align-items: center;\n}\n.shop-catalogue-overview-action + .shop-catalogue-overview-action {\n  margin-top: 60px;\n}\n.shop-catalogue-overview-action-icon {\n  width: 90px;\n  height: 90px;\n  line-height: 89px !important;\n  font-size: 22px;\n  text-align: center;\n  background: var(--color-bg-light);\n  border-radius: var(--radius);\n  transition: box-shadow 0.2s ease;\n  box-shadow: var(--shadow-short);\n}\n.shop-catalogue-overview-action-text {\n  line-height: 1.3;\n  color: var(--color-text-light);\n}\n.shop-catalogue-overview-action-text strong {\n  display: inline-block;\n  margin-bottom: 8px;\n  color: var(--color-text);\n  font-size: var(--font-size-l);\n}\n.shop-catalogue-tree-switch-search {\n  margin: 15px 15px 18px;\n}\n.shop-catalogue-tree-switch-search .ui-input {\n  min-width: 0 !important;\n}\n.shop-catalogue-tree-hint {\n  font-size: var(--font-size);\n  line-height: 1.5;\n  position: relative;\n  padding: 14px 20px;\n  margin: 0 var(--padding) 30px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  border: 1px solid var(--color-line);\n}\n.shop-catalogue-tree-hint a {\n  color: var(--color-text-dim);\n  text-decoration: underline dotted;\n  font-size: var(--font-size-s);\n}\n.shop-catalogue-tree-hint:before {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 26px;\n  height: 26px;\n  position: absolute;\n  left: -13px;\n  margin-top: -14px;\n  top: 50%;\n  border-radius: 20px;\n  background: var(--color-box);\n  border: 1px solid var(--color-line);\n  font-family: var(--font-icon);\n  font-size: 16px;\n  color: var(--color-text);\n  content: "\uE87F";\n  z-index: 1;\n}';
const script = {
  props: ["channelId", "id"],
  data: () => ({
    loaded: false,
    page: true,
    cache: {},
    resizable: {
      axis: "x",
      min: 260,
      max: 520,
      save: "shop-catalogue-tree",
      handle: ".ui-resizable"
    },
    channelSearch: null,
    channels: [],
    treeConfig: {},
    catalogues: [],
    selected: null,
    debounceChannelReload: null
  }),
  computed: {
    channelPreview() {
      if (this.selected) {
        return this.selected.name;
      }
      return "...";
    }
  },
  watch: {
    $route(to, from) {
      this.initialize();
    },
    channelSearch() {
      this.debounceChannelReload();
    }
  },
  created() {
    this.debounceChannelReload = debounce(this.reloadChannels, 200);
    this.initialize();
  },
  mounted() {
    hub.$off("shop.category.update");
    hub.$on("shop.category.update", (category) => {
      this.cache = [];
      this.$refs.tree.refresh();
    });
  },
  methods: {
    initialize() {
      if (!this.channelId && !this.$route.params.id) {
        ChannelsApi.getByIdOrDefault().then((res) => {
          this.onChannelChange(res.entity);
        });
      } else if (!this.loaded || this.catalogues.length < 1) {
        this.catalogues.push({
          id: "shared",
          name: "Catalogue",
          icon: "fth-folder",
          hidden: true,
          _catalogue: true
        });
        this.catalogues.push({
          id: this.zero.commerce.alias.localChannel,
          name: "Backlog",
          icon: "fth-folder",
          _catalogue: true
        });
        this.reloadChannels(() => this.setSelectedChannel());
        this.addCategoryTreeItem = {
          id: null,
          name: "Add",
          icon: "fth-plus"
        };
        this.loaded = true;
      } else {
        this.setSelectedChannel();
      }
      if (this.$refs.tree) {
        this.$refs.tree.refresh();
      }
    },
    reloadChannels(callback) {
      ChannelsApi.getForPicker(this.channelSearch).then((res) => {
        this.channels = res;
        if (callback) {
          callback(this.channels);
        }
      });
    },
    setSelectedChannel() {
      let channel = find(this.channels, (x) => x.id === this.channelId);
      let catalogue = find(this.catalogues, (x) => x.id === this.channelId);
      this.selected = channel || catalogue || this.channels[0];
    },
    getItems(parent) {
      var channelId = this.channelId;
      let key = channelId + "*" + (!parent ? "__root" : parent);
      if (this.cache[key]) {
        this.$route.params.channelId = this.cache[key].channelId;
        return Promise.resolve(this.cache[key].items);
      }
      return CategoriesApi.getChildren(channelId, parent, this.id).then((response) => {
        channelId = response.channelId;
        key = channelId + "*" + (!parent ? "__root" : parent);
        this.$route.params.channelId = channelId;
        response.items.forEach((item) => {
          item.url = {
            name: "commerce-categories-edit",
            params: {channelId, id: item.id}
          };
        });
        this.cache[key] = response;
        return response.items;
      });
    },
    onChannelChange(item) {
      this.channelSearch = null;
      let replace = ["commerce-categories"].indexOf(this.$route.name) > -1;
      this.$router[replace ? "replace" : "push"]({name: "commerce-categories", params: {channelId: item.id}});
    },
    create(parentId) {
      let channelId = this.channelId;
      Overlay.open({
        component: AddCategoryOverlay,
        model: {parentId, channelId},
        theme: "dark"
      }).then((item) => {
        this.cache = [];
        this.$router.push({
          name: "commerce-categories",
          params: {channelId, id: item.id}
        });
      }, () => {
      });
    },
    move(item) {
      return Overlay.open({
        component: MoveOverlay,
        display: "editor",
        model: item,
        channelId: this.channelId,
        categoryId: this.id
      }).then((value) => {
        hub.$emit("shop.category.move", value);
        hub.$emit("shop.category.update");
      });
    },
    sort(item) {
      return Overlay.open({
        component: SortOverlay,
        display: "editor",
        model: item,
        channelId: this.channelId,
        categoryId: this.id
      }).then((value) => {
        hub.$emit("shop.category.sort", value);
        hub.$emit("shop.category.update");
      });
    },
    remove(item) {
      Overlay.confirmDelete(item.name, "@shop.category.deleteoverlay.text").then((opts) => {
        opts.state("loading");
        CategoriesApi.delete(item.id).then((response) => {
          if (response.success) {
            opts.state("success");
            opts.hide();
            hub.$emit("shop.category.delete", response.model);
            hub.$emit("shop.category.update");
            Notification.success("@deleteoverlay.success", "@shop.category.deleteoverlay.success_text");
          } else {
            opts.errors(response.errors);
          }
        });
      });
    }
  }
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "../zero.Commerce/Plugin/pages/categories/categories.vue";
var categories = component.exports;
export default categories;

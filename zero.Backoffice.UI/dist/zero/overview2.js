import {n as normalizeComponent, C as CataloguesApi, N as Notification, A as Arrays, q as list, r as ProductsApi, O as Overlay, h as hub} from "./index.js";
import "./vendor.js";
var render$4 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "shop-cataloguecreate", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.catalogue.add_catalogue", expression: "'@shop.catalogue.add_catalogue'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "shop-cataloguecreate-items"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.item.name, expression: "item.name"}, {name: "localize", rawName: "v-localize:placeholder", value: "@shop.catalogue.fields.name_placeholder", expression: "'@shop.catalogue.fields.name_placeholder'", arg: "placeholder"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "200", readonly: _vm.disabled}, domProps: {value: _vm.item.name}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.item, "name", $event.target.value);
    }}})]), _c("div", {staticClass: "app-confirm-buttons"}, [!_vm.disabled ? _c("ui-button", {attrs: {type: "primary", submit: true, state: form.state, label: "@ui.create"}}) : _vm._e(), _c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel, disabled: _vm.loading}, on: {click: _vm.config.close}})], 1)];
  }}], null, false, 1761545563)}) : _vm._e();
};
var staticRenderFns$4 = [];
var create_vue_vue_type_style_index_0_lang$1 = ".shop-cataloguecreate {\n  text-align: left;\n}\n.shop-cataloguecreate-items {\n  margin-top: var(--padding);\n}\n.shop-cataloguecreate-items .ui-property + .ui-property,\n.shop-cataloguecreate-items .ui-split + .ui-property {\n  margin-top: 0;\n}";
const script$4 = {
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
      form.load(CataloguesApi.getEmpty()).then((response) => {
        this.disabled = false;
        this.item = response.entity;
        this.item.parentId = this.model.parentId;
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.handle(CataloguesApi.save(this.item)).then((response) => {
        this.config.confirm(response.model, this.config);
      });
    }
  }
};
const __cssModules$4 = {};
var component$4 = normalizeComponent(script$4, render$4, staticRenderFns$4, false, injectStyles$4, null, null, null);
function injectStyles$4(context) {
  for (let o in __cssModules$4) {
    this[o] = __cssModules$4[o];
  }
}
component$4.options.__file = "../zero.Commerce/Plugin/pages/products/catalogue/create.vue";
var AddCatalogueOverlay = component$4.exports;
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return !_vm.loading ? _c("ui-form", {ref: "form", staticClass: "shop-productcreate", scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.product.create.header", expression: "'@shop.product.create.header'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "shop-productcreate-items"}, _vm._l(_vm.actions, function(action) {
      return _c("button", {staticClass: "shop-productcreate-item", attrs: {type: "button"}, on: {click: function($event) {
        return _vm.selectAction(action.id);
      }}}, [_c("ui-icon", {staticClass: "shop-productcreate-item-icon", attrs: {symbol: action.icon, size: 24}}), _c("span", {staticClass: "shop-productcreate-item-text"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: action.prefix + action.id, expression: "action.prefix + action.id"}]}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: action.prefix + action.id + "_text", expression: "action.prefix + action.id + '_text'"}], staticClass: "-minor"})])], 1);
    }), 0)];
  }}], null, false, 4075674499)}) : _vm._e();
};
var staticRenderFns$3 = [];
var create_vue_vue_type_style_index_0_lang = ".shop-productcreate {\n  text-align: left;\n}\n.shop-productcreate-items {\n  margin: 24px -16px 0;\n}\n.shop-productcreate-item {\n  display: grid;\n  width: 100%;\n  transition: background 0.2s, transform 0.2s, opacity 0.2s;\n  grid-template-columns: 48px 1fr auto;\n  gap: 6px;\n  height: 100%;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n  padding: 16px;\n  border-radius: var(--radius);\n}\n.shop-productcreate-item:hover, .shop-productcreate-item:focus {\n  background: var(--color-box-nested);\n}\n.shop-productcreate-item:hover .shop-productcreate-item-icon, .shop-productcreate-item:focus .shop-productcreate-item-icon {\n  color: var(--color-text);\n}\n.shop-productcreate-item + .shop-productcreate-item {\n  margin-top: 10px;\n}\n.shop-productcreate-item-text {\n  display: flex;\n  flex-direction: column;\n  line-height: 1.3;\n}\n.shop-productcreate-item-text .-minor {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.shop-productcreate-item-icon {\n  font-size: 22px;\n  line-height: 1;\n  font-weight: 400;\n  position: relative;\n  top: -2px;\n  left: 4px;\n  color: var(--color-text);\n  transition: color 0.2s ease;\n}";
const script$3 = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: false,
    item: {},
    disabled: false,
    actions: [
      {id: "physical", icon: "fth-package", prefix: "@shop.product.type_states."},
      {id: "digital", icon: "fth-download-cloud", prefix: "@shop.product.type_states."},
      {id: "giftCard", icon: "fth-gift", prefix: "@shop.product.type_states."},
      {id: "copy", icon: "fth-copy", prefix: "@shop.product.create."}
    ]
  }),
  methods: {
    selectAction(type) {
      this.config.confirm({type}, this.config);
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
component$3.options.__file = "../zero.Commerce/Plugin/pages/products/overlays/create.vue";
var AddProductOverlay = component$3.exports;
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
var move_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.shop-catalogues-move .ui-box {\n  margin: 0;\n  padding: 16px 0;\n}\n.shop-catalogues-move .ui-box .ui-tree-item.is-disabled {\n  opacity: 0.5;\n}\n.shop-catalogues-move .ui-box .ui-tree-item.is-selected, .shop-catalogues-move .ui-box .ui-tree-item:hover:not(.is-disabled) {\n  background: var(--color-tree-selected);\n}\n.shop-catalogues-move .ui-box .ui-tree-item.is-selected:after {\n  font-family: "Feather";\n  content: "\uE83E";\n  font-size: 16px;\n  color: var(--color-primary);\n}\n.shop-catalogues-move .ui-box .ui-tree-item.is-selected .ui-tree-item-text {\n  font-weight: bold;\n}\n.shop-catalogues-move content {\n  padding-top: 0;\n}\n.shop-catalogues-move-text {\n  margin: 0 0 20px;\n}';
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
      return CataloguesApi.getChildren(parent, this.model.parentId).then((response) => {
        let items = response.filter((x) => x.id);
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
          item.disabled = item.id == this.model.id;
          item.hasActions = false;
        });
        return items;
      });
    },
    onSave() {
      if (this.model.parentId == this.selected.id) {
        this.config.close();
        return;
      }
      this.state = "loading";
      CataloguesApi.move(this.model.id, this.selected.id).then((res) => {
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
component$2.options.__file = "../zero.Commerce/Plugin/pages/products/catalogue/move.vue";
var MoveOverlay = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-catalogues-sort", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.sort.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.save", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize", value: "@ui.sort.text", expression: "'@ui.sort.text'"}], staticClass: "shop-catalogues-sort-text"}), _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {handle: ".is-handle", onUpdate: _vm.onSortingUpdated}, expression: "{ handle: '.is-handle', onUpdate: onSortingUpdated }"}], staticClass: "shop-catalogues-sort-items"}, _vm._l(_vm.items, function(item, index) {
    return _c("div", {key: item.id, staticClass: "shop-catalogues-sort-item"}, [_c("span", [_vm._v(_vm._s(index + 1) + ". "), _c("i", {staticClass: "shop-catalogues-sort-item-icon", class: item.icon}), _vm._v(" " + _vm._s(item.name))]), _c("button", {staticClass: "shop-catalogues-sort-item-button is-handle", attrs: {type: "button"}}, [_c("ui-icon", {staticClass: "-minor", attrs: {symbol: "fth-more-vertical", size: 14}})], 1)]);
  }), 0)]);
};
var staticRenderFns$1 = [];
var sort_vue_vue_type_style_index_0_lang = ".shop-catalogues-sort .ui-box {\n  margin: 0;\n}\n.shop-catalogues-sort content {\n  padding-top: 0;\n}\n.shop-catalogues-sort-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 1fr auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  height: 46px;\n  color: var(--color-text);\n  position: relative;\n  padding: 0 8px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n}\n.shop-catalogues-sort-item i {\n  font-size: var(--font-size-l);\n  position: relative;\n  top: -1px;\n  color: var(--color-text-dim);\n}\n.shop-catalogues-sort-item span {\n  padding: 12px 8px;\n}\n.shop-catalogues-sort-item.is-selected {\n  color: var(--color-text-dim);\n}\n.shop-catalogues-sort-item + .shop-catalogues-sort-item {\n  margin-top: 8px;\n}\nbutton.shop-catalogues-sort-item-button {\n  height: 48px;\n  width: 24px;\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  text-align: center;\n}\nbutton.shop-catalogues-sort-item-button i {\n  font-size: var(--font-size);\n}\nbutton.shop-catalogues-sort-item-button.is-handle {\n  cursor: move;\n}\n.shop-catalogues-sort-text {\n  margin: 0 0 20px;\n}\ni.shop-catalogues-sort-item-icon {\n  top: 0;\n  color: var(--color-text);\n  margin-right: 8px;\n}";
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
    return CataloguesApi.getChildren(this.parentId).then((response) => {
      this.items = response.filter((x) => x.id);
    });
  },
  methods: {
    onSave() {
      this.state = "loading";
      CataloguesApi.saveSorting(this.items.map((x) => x.id)).then((res) => {
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
component$1.options.__file = "../zero.Commerce/Plugin/pages/products/catalogue/sort.vue";
var SortOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-catalogue"}, [_c("div", {directives: [{name: "resizable", rawName: "v-resizable", value: _vm.resizable, expression: "resizable"}], staticClass: "app-tree shop-catalogue-tree"}, [_c("ui-header-bar", {staticClass: "ui-tree-header", attrs: {title: "@shop.catalogue.list", "back-button": false}}, [_c("ui-dot-button", {on: {click: function($event) {
    return _vm.$refs.tree.onActionsClicked(null, $event);
  }}})], 1), _c("ui-tree", {ref: "tree", attrs: {get: _vm.getTreeItems, config: _vm.treeConfig, active: _vm.id}, scopedSlots: _vm._u([{key: "actions", fn: function(props) {
    return [_c("ui-dropdown-button", {attrs: {label: "@ui.create", icon: "fth-plus"}, on: {click: function($event) {
      return _vm.create(props.item != null ? props.item.id : null);
    }}}), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
      return _vm.move(props.item);
    }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.sort.title", icon: "fth-arrow-down"}, on: {click: function($event) {
      return _vm.sort(props.item);
    }}}), props.item ? _c("ui-dropdown-separator") : _vm._e(), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash"}, on: {click: function($event) {
      return _vm.remove(props.item);
    }}}) : _vm._e()];
  }}, {key: "bottom", fn: function() {
    return [_c("ui-tree-item", {attrs: {value: _vm.addCatalogueTreeItem}, on: {click: function($event) {
      return _vm.create(null);
    }}})];
  }, proxy: true}])}), _c("div", {staticClass: "app-tree-resizable ui-resizable"})], 1), _c("main", {staticClass: "shop-catalogue-main"}, [_c("ui-header-bar", {attrs: {title: "@shop.catalogue.products", count: _vm.count}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {decision: false}, on: {click: _vm.onProductCreate}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "commerce.products"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1)]);
};
var staticRenderFns = [];
var overview_vue_vue_type_style_index_0_lang = '@charset "UTF-8";\n.shop-catalogue {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 2px;\n  justify-content: stretch;\n  height: 100vh;\n}\n.shop-catalogue-main {\n  max-height: 100%;\n  font-size: var(--font-size);\n  overflow-y: auto;\n}\n.app-tree.shop-catalogue-tree {\n  overflow: visible;\n  display: flex;\n  flex-direction: column;\n}\n.app-tree.shop-catalogue-tree .ui-header-bar {\n  margin-bottom: 0;\n  flex-shrink: 0;\n}\n.app-tree.shop-catalogue-tree .ui-tree {\n  overflow-y: auto;\n}\n.shop-catalogue-overview {\n  padding: 95px 0 0 60px;\n}\n.shop-catalogue-overview-action {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 35px;\n  align-items: center;\n}\n.shop-catalogue-overview-action + .shop-catalogue-overview-action {\n  margin-top: 60px;\n}\n.shop-catalogue-overview-action-icon {\n  width: 90px;\n  height: 90px;\n  line-height: 89px !important;\n  font-size: 22px;\n  text-align: center;\n  background: var(--color-bg-light);\n  border-radius: var(--radius);\n  transition: box-shadow 0.2s ease;\n  box-shadow: var(--shadow-short);\n}\n.shop-catalogue-overview-action-text {\n  line-height: 1.3;\n  color: var(--color-text-light);\n}\n.shop-catalogue-overview-action-text strong {\n  display: inline-block;\n  margin-bottom: 8px;\n  color: var(--color-text);\n  font-size: var(--font-size-l);\n}\n.shop-catalogue-tree-hint {\n  font-size: var(--font-size);\n  line-height: 1.5;\n  position: relative;\n  padding: 14px 20px;\n  margin: 0 var(--padding) 30px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  border: 1px solid var(--color-line);\n}\n.shop-catalogue-tree-hint a {\n  color: var(--color-text-dim);\n  text-decoration: underline dotted;\n  font-size: var(--font-size-s);\n}\n.shop-catalogue-tree-hint:before {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 26px;\n  height: 26px;\n  position: absolute;\n  left: -13px;\n  margin-top: -14px;\n  top: 50%;\n  border-radius: 20px;\n  background: var(--color-box);\n  border: 1px solid var(--color-line);\n  font-family: var(--font-icon);\n  font-size: 16px;\n  color: var(--color-text);\n  content: "\uE87F";\n  z-index: 1;\n}\n\n/*.shop-catalogue .ui-table-field-image\n{\n  mix-blend-mode: multiply;\n}*/\n.shop-catalogue .ui-table-cell[table-field=image]:first-child {\n  padding: 11px 0 10px 12px;\n  justify-content: center;\n}\n.shop-catalogue .ui-table-cell[table-field=image]:first-child img {\n  max-height: 40px;\n  max-width: 40px;\n}\n.shop-catalogue-tree > .ui-tree > .ui-tree-item:first-child > .ui-tree-item-link.is-active:not(.is-active-exact) {\n  font-weight: 400;\n}\n.shop-catalogue .ui-tree-item {\n  height: 62px;\n}\n.shop-catalogue .ui-tree-item {\n  border-top: 1px solid var(--color-line);\n}\n.shop-catalogue-tree.app-tree .ui-header-bar + .ui-tree {\n  margin-top: 0;\n}';
const script = {
  props: ["id"],
  data: () => ({
    count: 0,
    list,
    resizable: {
      axis: "x",
      min: 260,
      max: 520,
      save: "shop-catalogue-tree",
      handle: ".ui-resizable"
    },
    treeConfig: {},
    addCatalogueTreeItem: {
      id: null,
      name: "Add",
      icon: "fth-plus"
    }
  }),
  watch: {
    $route(to, from) {
      this.reload();
    }
  },
  created() {
    this.reload();
  },
  methods: {
    reload() {
      this.list.onFetch((query) => ProductsApi.getByCatalogue(this.id, query));
    },
    getTreeItems(parent) {
      this.channelId;
      return CataloguesApi.getChildren(parent, this.id).then((response) => {
        response.forEach((item) => {
          item.url = {
            name: "commerce-products",
            params: {id: item.id}
          };
        });
        return response;
      });
    },
    onProductCreate() {
      Overlay.open({
        component: AddProductOverlay,
        model: {
          catalogueId: this.id
        },
        theme: "dark"
      }).then((item) => {
        console.info(item);
        this.$router.push({
          name: "commerce-products-create",
          params: {
            type: item.type,
            catalogueId: this.id
          }
        });
      }, () => {
      });
    },
    create(parentId) {
      Overlay.open({
        component: AddCatalogueOverlay,
        model: {parentId},
        theme: "dark"
      }).then((item) => {
        this.cache = [];
        this.$router.push({
          name: "commerce-products",
          params: {id: item.id}
        });
      }, () => {
      });
    },
    move(item) {
      return Overlay.open({
        component: MoveOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("shop.catalogue.move", value);
        hub.$emit("shop.catalogue.update");
      });
    },
    sort(item) {
      return Overlay.open({
        component: SortOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("shop.catalogue.sort", value);
        hub.$emit("shop.catalogue.update");
      });
    },
    remove(item) {
      Overlay.confirmDelete(item.name, "@shop.catalogue.deleteoverlay.text").then((opts) => {
        opts.state("loading");
        CataloguesApi.delete(item.id).then((response) => {
          if (response.success) {
            opts.state("success");
            opts.hide();
            hub.$emit("shop.catalogue.delete", response.model);
            hub.$emit("shop.catalogue.update");
            Notification.success("@deleteoverlay.success", "@shop.catalogue.deleteoverlay.success_text");
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
component.options.__file = "../zero.Commerce/Plugin/pages/products/overview.vue";
var overview = component.exports;
export default overview;

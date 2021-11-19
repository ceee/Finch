import {n as normalizeComponent, a as PagesApi, P as PageTreeApi, N as Notification, A as Arrays, h as hub, O as Overlay} from "./index.js";
import {M as MoveOverlay, C as CopyOverlay} from "./copy.js";
import "./vendor.js";
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "page-create"}, [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@page.create.title", expression: "'@page.create.title'"}], staticClass: "ui-headline"}), !_vm.loading ? _c("div", [_vm.pageTypes.length && _vm.config.parent ? _c("div", {staticClass: "page-create-parent"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@page.create.parent", expression: "'@page.create.parent'"}]}), _vm._v(": "), _c("strong", [_vm._v(_vm._s(_vm.config.parent.name))])]) : _vm._e(), _c("div", {staticClass: "page-create-items"}, _vm._l(_vm.pageTypes, function(item) {
    return _c("button", {staticClass: "page-create-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.onSelect(item);
    }}}, [_c("ui-icon", {staticClass: "page-create-item-icon", attrs: {symbol: item.icon, size: 22}}), _c("span", {staticClass: "page-create-item-text"}, [_c("ui-localize", {attrs: {value: item.name}}), item.description ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.description, expression: "item.description"}], staticClass: "page-create-item-description"}) : _vm._e()], 1)], 1);
  }), 0), !_vm.pageTypes.length ? _c("ui-message", {attrs: {type: "error", text: "@page.create.nonavailable"}}) : _vm._e(), _c("div", {staticClass: "app-confirm-buttons"}, [_c("ui-button", {attrs: {type: "light", label: _vm.config.closeLabel}, on: {click: _vm.config.close}})], 1)], 1) : _vm._e()]);
};
var staticRenderFns$2 = [];
var create_vue_vue_type_style_index_0_lang = ".page-create {\n  text-align: left;\n}\n.page-create .ui-message {\n  margin: 0;\n}\n.page-create-parent {\n  margin: 30px 0 -10px 0;\n  border-radius: var(--radius);\n  /*border: 1px solid var(--color-line-light);*/\n  background: var(--color-box-nested);\n  line-height: 1.4;\n  color: var(--color-text-dim);\n  padding: 14px 16px;\n  font-size: var(--font-size);\n}\n.page-create-parent strong {\n  color: var(--color-text);\n}\n.page-create-items {\n  margin: 0 -16px;\n  margin-top: var(--padding);\n  max-height: 490px;\n  overflow-y: auto;\n}\n.page-create-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 40px 1fr auto;\n  gap: 12px;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n  padding: 16px;\n  border-radius: var(--radius);\n}\n.page-create-item:hover, .page-create-item:focus {\n  background: var(--color-tree-selected);\n}\n.page-create-item + .page-create-item {\n  margin-top: 2px;\n}\n.page-create-item-text {\n  display: flex;\n  flex-direction: column;\n}\n.page-create-item-description {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.page-create-item-icon {\n  position: relative;\n  top: -2px;\n  left: 4px;\n  color: var(--color-text);\n}";
const script$2 = {
  props: {
    config: Object
  },
  data: () => ({
    model: {
      name: null,
      parentId: null,
      pageTypeAlias: null
    },
    loading: false,
    item: {},
    disabled: false,
    pageTypes: []
  }),
  created() {
    this.model.parentId = this.config.parent ? this.config.parent.id : null;
  },
  mounted() {
    this.loading = true;
    PagesApi.getAllowedPageTypes(this.model.parentId).then((response) => {
      this.pageTypes = response;
      this.loading = false;
    });
  },
  methods: {
    onSelect(item) {
      this.config.close();
      this.$router.push({
        name: "page-create",
        params: {type: item.alias, parent: this.model.parentId}
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
component$2.options.__file = "app/pages/pages/overlays/create.vue";
var CreateOverlay = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "pages-sort", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.sort.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.save", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize", value: "@ui.sort.text", expression: "'@ui.sort.text'"}], staticClass: "pages-sort-text"}), _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {handle: ".is-handle", onUpdate: _vm.onSortingUpdated}, expression: "{ handle: '.is-handle', onUpdate: onSortingUpdated }"}], staticClass: "pages-sort-items"}, _vm._l(_vm.items, function(item, index) {
    return _c("div", {key: item.id, staticClass: "pages-sort-item"}, [_c("span", [_vm._v(_vm._s(index + 1) + ". "), _c("ui-icon", {staticClass: "pages-sort-item-icon", attrs: {symbol: item.icon, size: 15}}), _vm._v(" " + _vm._s(item.name))], 1), _c("button", {staticClass: "pages-sort-item-button is-handle", attrs: {type: "button"}}, [_c("ui-icon", {staticClass: "-minor", attrs: {symbol: "fth-more-vertical", size: 14}})], 1)]);
  }), 0)]);
};
var staticRenderFns$1 = [];
var sort_vue_vue_type_style_index_0_lang = ".pages-sort .ui-box {\n  margin: 0;\n}\n.pages-sort content {\n  padding-top: 0;\n}\n.pages-sort-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 1fr auto;\n  gap: 6px;\n  align-items: center;\n  font-size: var(--font-size);\n  height: 46px;\n  color: var(--color-text);\n  position: relative;\n  padding: 0 8px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n}\n.pages-sort-item i {\n  font-size: var(--font-size-l);\n  position: relative;\n  top: -1px;\n  color: var(--color-text-dim);\n}\n.pages-sort-item span {\n  padding: 12px 8px;\n}\n.pages-sort-item.is-selected {\n  color: var(--color-text-dim);\n}\n.pages-sort-item + .pages-sort-item {\n  margin-top: 8px;\n}\nbutton.pages-sort-item-button {\n  height: 48px;\n  width: 24px;\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  text-align: center;\n}\nbutton.pages-sort-item-button i {\n  font-size: var(--font-size);\n}\nbutton.pages-sort-item-button.is-handle {\n  cursor: move;\n}\n.pages-sort-text {\n  margin: 0 0 20px;\n}\n.pages-sort-item-icon {\n  position: relative;\n  top: 2px;\n  color: var(--color-text);\n  margin: 0 6px;\n}";
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
    return PageTreeApi.getChildren(this.parentId).then((response) => {
      this.items = response.filter((x) => x.id !== "recyclebin");
    });
  },
  methods: {
    onSave() {
      this.state = "loading";
      PagesApi.saveSorting(this.items.map((x) => x.id)).then((res) => {
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
component$1.options.__file = "app/pages/pages/overlays/sort.vue";
var SortOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "page-container"}, [_c("div", {directives: [{name: "resizable", rawName: "v-resizable", value: _vm.resizable, expression: "resizable"}], ref: "scrollable", staticClass: "app-tree"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, config: _vm.treeConfig, active: _vm.id, header: "Pages"}, on: {setactive: _vm.onActiveSet}, scopedSlots: _vm._u([{key: "actions", fn: function(props) {
    return [!props.item || props.id !== "recyclebin" ? [_c("ui-dropdown-button", {attrs: {label: "@ui.create", icon: "fth-plus"}, on: {click: function($event) {
      return _vm.create(props.item);
    }}}), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.move.title", icon: "fth-corner-down-right"}, on: {click: function($event) {
      return _vm.move(props.item);
    }}}) : _vm._e(), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.copy.title", icon: "fth-copy"}, on: {click: function($event) {
      return _vm.copy(props.item);
    }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@ui.sort.title", icon: "fth-arrow-down"}, on: {click: function($event) {
      return _vm.sort(props.item);
    }}}), props.item ? _c("ui-dropdown-separator") : _vm._e(), props.item ? _c("ui-dropdown-button", {attrs: {label: "@ui.delete", icon: "fth-trash"}, on: {click: function($event) {
      return _vm.remove(props.item);
    }}}) : _vm._e()] : _vm._e()];
  }}])}), _c("div", {staticClass: "app-tree-resizable ui-resizable"})], 1), !_vm.isOverview ? _c("router-view") : _vm._e(), _vm.isOverview ? _c("div", {staticClass: "page-overview"}, _vm._l(_vm.actions, function(action) {
    return _c("button", {key: action.alias, staticClass: "page-overview-action", attrs: {type: "button"}, on: {click: function($event) {
      return action.action(action);
    }}}, [_c("div", {staticClass: "page-overview-action-icon"}, [_c("ui-icon", {attrs: {symbol: action.icon, size: 24}})], 1), _c("p", {staticClass: "page-overview-action-text"}, [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: "@page.overview.actions." + action.alias, expression: "'@page.overview.actions.' + action.alias"}]}), _c("br"), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@page.overview.actions." + action.alias + "_text", tokens: action.tokens}, expression: "{ key: '@page.overview.actions.' + action.alias + '_text', tokens: action.tokens }"}]})])]);
  }), 0) : _vm._e()], 1);
};
var staticRenderFns = [];
var pages_vue_vue_type_style_index_0_lang = ".page-container {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 2px;\n  justify-content: stretch;\n  height: calc(100vh - 70px);\n}\n.page-overview {\n  display: flex;\n  flex-direction: column;\n  margin-left: 80px;\n  padding-top: 115px;\n}\n.page-overview-action {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: 35px;\n  align-items: center;\n}\n.page-overview-action + .page-overview-action {\n  margin-top: var(--padding);\n}\n.page-overview-action-icon {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 90px;\n  height: 90px;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n}\n.page-overview-action-text {\n  line-height: 1.3;\n  color: var(--color-text-dim);\n}\n.page-overview-action-text strong {\n  display: inline-block;\n  margin-bottom: 8px;\n  color: var(--color-text);\n  font-size: var(--font-size-l);\n}";
const script = {
  data: () => ({
    page: true,
    cache: {},
    resizable: {
      axis: "x",
      min: 260,
      max: 520,
      save: "page-tree",
      handle: ".ui-resizable"
    },
    actions: [],
    treeConfig: {}
  }),
  computed: {
    id() {
      return this.$route.params.id;
    },
    isOverview() {
      return !this.$route.params.id && !this.$route.params.type && this.$route.name !== "recyclebin";
    }
  },
  mounted() {
    this.buildActions();
    hub.$off("page.update");
    hub.$on("page.update", (page) => {
      this.cache = [];
      this.$refs.tree.refresh();
    });
  },
  methods: {
    onActiveSet(val) {
      this.$nextTick(() => {
        let container = this.$refs.scrollable;
        let child = val.$el;
        container.offsetHeight * 0.5 - child.offsetHeight * 0.5;
        let threshold = container.offsetHeight * 0.1;
        ({
          from: container.scrollTop + threshold,
          to: container.scrollTop + container.offsetHeight - threshold
        });
        let rect = child.getBoundingClientRect();
        rect.top + container.offsetTop;
        let scrollTo = child.offsetTop;
        container.scrollTo({top: scrollTo, behavior: "smooth"});
      });
    },
    getItems(parent) {
      const key = !parent ? "__root" : parent;
      if (this.cache[key]) {
        return Promise.resolve(this.cache[key]);
      }
      return PageTreeApi.getChildren(parent, this.id).then((response) => {
        response.forEach((item) => {
          item.url = {
            name: "page",
            params: {id: item.id}
          };
          if (item.id === "recyclebin") {
            item.hasActions = false;
            item.url = {
              name: "recyclebin"
            };
          }
        });
        this.cache[key] = response;
        return response;
      });
    },
    create(parent) {
      Overlay.open({
        component: CreateOverlay,
        width: 480,
        parent
      }).then(() => {
      }, () => {
      });
    },
    sort(item) {
      return Overlay.open({
        component: SortOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("page.sort", value);
        hub.$emit("page.update");
      });
    },
    move(item) {
      return Overlay.open({
        component: MoveOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("page.move", value);
        hub.$emit("page.update");
      });
    },
    copy(item) {
      console.info(item);
      return Overlay.open({
        component: CopyOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        hub.$emit("page.update");
        this.$router.push({
          name: "page",
          params: {id: value.id}
        });
      });
    },
    remove(item) {
      Overlay.confirmDelete(item.name, "@deleteoverlay.page_text").then((opts) => {
        opts.state("loading");
        PagesApi.delete(item.id).then((response) => {
          if (response.success) {
            opts.state("success");
            opts.hide();
            hub.$emit("page.delete", response.model);
            hub.$emit("page.update");
            Notification.success("@deleteoverlay.success", "@deleteoverlay.page_success_text");
          } else {
            opts.errors(response.errors);
          }
        });
      });
    },
    buildActions() {
      this.actions = [];
      var instance = this;
      let lastEditedPageId = localStorage.getItem("zero.last-page." + zero.appId);
      if (lastEditedPageId) {
        PagesApi.getById(lastEditedPageId).then((res) => {
          this.actions.push({
            alias: "continue",
            icon: "fth-corner-down-right",
            tokens: {
              page: res.entity.name
            },
            action() {
              instance.$router.push({
                name: "page",
                params: {id: res.entity.id}
              });
            }
          });
        });
      }
      this.actions.push({
        alias: "new",
        icon: "fth-plus",
        tokens: {
          root: "Home"
        },
        action() {
          instance.create();
        }
      });
      this.actions.push({
        alias: "history",
        icon: "fth-clock",
        action() {
          Notification.error("Not implemented", "Page editing history has not been implemented yet");
        }
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
component.options.__file = "app/pages/pages/pages.vue";
var pages = component.exports;
export default pages;

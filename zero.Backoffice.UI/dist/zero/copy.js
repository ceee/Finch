import {n as normalizeComponent, P as PageTreeApi, a as PagesApi, N as Notification} from "./index.js";
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "pages-move", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.move.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.move.action", state: _vm.state}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: "@ui.move.text", tokens: {name: _vm.model.name}}, expression: "{ key: '@ui.move.text', tokens: { name: model.name } }", arg: "html"}], staticClass: "pages-move-text"}), _c("div", {staticClass: "ui-box pages-move-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, mode: "select"}, on: {select: _vm.onSelect}})], 1)]);
};
var staticRenderFns$1 = [];
var move_vue_vue_type_style_index_0_lang = ".pages-move .ui-box {\n  margin: 0;\n  padding: 16px 0;\n}\n.pages-move .ui-box .ui-tree-item.is-disabled {\n  opacity: 0.5;\n}\n.pages-move content {\n  padding-top: 0;\n}\n.pages-move-text {\n  margin: 0 0 20px;\n}";
const script$1 = {
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
    onSelect(id) {
      this.selected = id;
    },
    getItems(parent) {
      const key = !parent ? "__root" : parent;
      if (this.cache[key]) {
        return Promise.resolve(this.cache[key]);
      }
      return PageTreeApi.getChildren(parent).then((response) => {
        if (!parent) {
          response.splice(0, 0, {
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
        response.forEach((item) => {
          item.disabled = item.id === "recyclebin" || item.id == this.model.id;
          item.hasActions = false;
        });
        this.cache[key] = response;
        return response;
      });
    },
    onSave() {
      if (this.model.parentId == this.selected) {
        this.config.close();
        return;
      }
      this.state = "loading";
      PagesApi.move(this.model.id, this.selected).then((res) => {
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
const __cssModules$1 = {};
var component$1 = normalizeComponent(script$1, render$1, staticRenderFns$1, false, injectStyles$1, null, null, null);
function injectStyles$1(context) {
  for (let o in __cssModules$1) {
    this[o] = __cssModules$1[o];
  }
}
component$1.options.__file = "app/pages/pages/overlays/move.vue";
var MoveOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "pages-copy", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@ui.copy.title", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", label: "@ui.copy.action", state: _vm.state, disabled: _vm.selected == null}, on: {click: _vm.onSave}})];
  }, proxy: true}])}, [_c("p", {directives: [{name: "localize", rawName: "v-localize:html", value: {key: "@ui.copy.text", tokens: {name: _vm.model.name}}, expression: "{ key: '@ui.copy.text', tokens: { name: model.name } }", arg: "html"}], staticClass: "pages-copy-text"}), _c("div", {staticClass: "ui-box"}, [_c("ui-property", {attrs: {label: "@ui.copy.includeDescendants"}}, [_c("ui-toggle", {staticClass: "is-primary", model: {value: _vm.includeDescendants, callback: function($$v) {
    _vm.includeDescendants = $$v;
  }, expression: "includeDescendants"}})], 1)], 1), _c("div", {staticClass: "ui-box pages-copy-items"}, [_c("ui-tree", {ref: "tree", attrs: {get: _vm.getItems, mode: "select"}, on: {select: _vm.onSelect}})], 1)]);
};
var staticRenderFns = [];
var copy_vue_vue_type_style_index_0_lang = ".pages-copy .ui-property {\n  display: flex;\n  justify-content: space-between;\n}\n.pages-copy .ui-property-content {\n  display: inline;\n  flex: 0 0 auto;\n}\n.pages-copy .ui-property-label {\n  padding-top: 1px;\n}\n.pages-copy .ui-box {\n  margin: 0;\n  padding: 20px var(--padding) 18px;\n}\n.pages-copy .ui-box + .ui-box {\n  padding: 16px 0;\n  margin-top: 26px;\n}\n.pages-copy .ui-box .ui-tree-item.is-disabled {\n  opacity: 0.5;\n}\n.pages-copy content {\n  padding-top: 0;\n}\n.pages-copy-text {\n  margin: 0 0 20px;\n}";
const script = {
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
    state: "default",
    cache: {},
    prevItem: null,
    selected: null,
    includeDescendants: true
  }),
  mounted() {
  },
  methods: {
    onSelect(id) {
      this.selected = id;
    },
    getItems(parent) {
      const key = !parent ? "__root" : parent;
      if (this.cache[key]) {
        return Promise.resolve(this.cache[key]);
      }
      return PageTreeApi.getChildren(parent).then((response) => {
        if (!parent) {
          response.splice(0, 0, {
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
        response.forEach((item) => {
          item.disabled = item.id === "recyclebin" || item.id == this.model.id;
          item.hasActions = false;
        });
        this.cache[key] = response;
        return response;
      });
    },
    onSave() {
      this.state = "loading";
      PagesApi.copy(this.model.id, this.selected, this.includeDescendants).then((res) => {
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
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "app/pages/pages/overlays/copy.vue";
var CopyOverlay = component.exports;
export {CopyOverlay as C, MoveOverlay as M};

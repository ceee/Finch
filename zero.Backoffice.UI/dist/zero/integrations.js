var __defProp = Object.defineProperty;
var __hasOwnProp = Object.prototype.hasOwnProperty;
var __getOwnPropSymbols = Object.getOwnPropertySymbols;
var __propIsEnum = Object.prototype.propertyIsEnumerable;
var __defNormalProp = (obj, key, value) => key in obj ? __defProp(obj, key, {enumerable: true, configurable: true, writable: true, value}) : obj[key] = value;
var __assign = (a, b) => {
  for (var prop in b || (b = {}))
    if (__hasOwnProp.call(b, prop))
      __defNormalProp(a, prop, b[prop]);
  if (__getOwnPropSymbols)
    for (var prop of __getOwnPropSymbols(b)) {
      if (__propIsEnum.call(b, prop))
        __defNormalProp(a, prop, b[prop]);
    }
  return a;
};
import {g as get, p as post, d as del, n as normalizeComponent, O as Overlay, i as __vitePreload, N as Notification} from "./index.js";
const base = "integrations/";
var IntegrationsApi = {
  getEmpty: async (alias) => await get(base + "getEmpty", {params: {alias}}),
  getTypes: async () => await get(base + "getTypes"),
  getByAlias: async (alias, config) => await get(base + "getByAlias", __assign(__assign({}, config), {params: {alias}})),
  getByQuery: async (query, config) => await get(base + "getByQuery", __assign(__assign({}, config), {params: {query}})),
  save: async (model, config) => await post(base + "save", model, __assign({}, config)),
  saveActiveState: async (model, config) => await post(base + "saveActiveState", model, __assign({}, config)),
  delete: async (alias) => await del(base + "delete", {params: {alias}})
};
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "integrations-item", class: {"is-configured": _vm.model.isConfigured}}, [_c("aside", [_c("span", {staticClass: "integrations-item-icon", style: {"background-color": _vm.hasColor ? _vm.model.type.color : null}}, [_c("ui-icon", {attrs: {symbol: _vm.model.type.icon || "fth-box", size: 26}})], 1), !_vm.model.isConfigured ? _c("button", {staticClass: "ui-button type-primary type-block", attrs: {type: "button"}, on: {click: _vm.open}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "Setup", expression: "'Setup'"}], staticClass: "ui-button-text"})]) : _c("button", {staticClass: "ui-button type-primary type-block", attrs: {type: "button"}, on: {click: _vm.open}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "Edit", expression: "'Edit'"}], staticClass: "ui-button-text"})])]), _c("main", [_c("p", {staticClass: "integrations-item-text"}, [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: _vm.model.type.name, expression: "model.type.name"}]}), _vm.model.isConfigured ? _c("ui-icon", {attrs: {symbol: "fth-check-circle", size: 13}}) : _vm._e(), _vm.model.type.description ? [_c("br"), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.model.type.description, expression: "model.type.description"}]})] : _vm._e()], 2), _c("div", {staticClass: "integrations-item-tags"}, _vm._l(_vm.model.type.tags, function(tag) {
    return _c("span", {staticClass: "ui-tag"}, [_vm._v(_vm._s(tag))]);
  }), 0), _vm.model.isConfigured ? _c("ui-toggle", {staticClass: "integrations-item-toggle", attrs: {"on-content": "Active", "off-content": "Active"}, on: {input: function($event) {
    return _vm.$emit("onActiveChange", _vm.model);
  }}, model: {value: _vm.model.isActive, callback: function($$v) {
    _vm.$set(_vm.model, "isActive", $$v);
  }, expression: "model.isActive"}}) : _vm._e()], 1)]);
};
var staticRenderFns$1 = [];
var integrationsItem_vue_vue_type_style_index_0_lang = ".integrations-item {\n  color: var(--color-text);\n  font-size: var(--font-size);\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: var(--padding);\n  align-items: flex-start;\n}\n.integrations-item .ui-button.type-block {\n  justify-content: center;\n  margin-top: 5px;\n}\n.integrations-item + .integrations-item {\n  margin-top: var(--padding-m);\n  border-top: 1px solid var(--color-line);\n  padding-top: var(--padding-m);\n}\n.integrations-item-tags {\n  margin-top: var(--padding-s);\n}\n.integrations-item-icon {\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  width: 120px;\n  height: 90px;\n  font-size: 22px;\n  text-align: center;\n  background: var(--color-box-nested);\n  border-radius: var(--radius);\n}\n.integrations-item-icon.has-color {\n  color: white;\n  box-shadow: none;\n}\n.integrations-item-text {\n  line-height: 1.3;\n  color: var(--color-text-dim);\n  margin: 0.5em 0 0;\n  max-width: 820px;\n}\n.integrations-item-text .ui-icon {\n  color: var(--color-primary);\n  margin-left: 0.5em;\n  font-size: 1.1em;\n}\n.integrations-item-text strong {\n  display: inline-block;\n  margin-bottom: 5px;\n  color: var(--color-text);\n  font-size: var(--font-size);\n}\n.integrations-item-toggle {\n  margin-top: var(--padding-m);\n}";
const script$1 = {
  props: {
    model: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    active: false
  }),
  computed: {
    hasColor() {
      return !!this.model.type.color && this.model.isConfigured;
    }
  },
  methods: {
    open() {
      return Overlay.open({
        component: () => __vitePreload(() => __import__("./integration.js"), true ? ["/zero/integration.js","/zero/index.js","/zero/index.css","/zero/vendor.js"] : void 0),
        display: "editor",
        model: this.model.type,
        isCreate: !this.model.isConfigured,
        alias: this.model.type.alias,
        width: 960
      }).then((value) => {
        this.$emit("change", value);
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
component$1.options.__file = "app/pages/settings/integrations-item.vue";
var IntegrationItem = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "integrations"}, [_c("ui-header-bar", {attrs: {title: "@integration.list", count: _vm.count, "back-button": true}}), _c("div", {staticClass: "ui-box"}, _vm._l(_vm.items, function(item, index) {
    return _c("integration-item", {key: index, attrs: {model: item}, on: {change: _vm.onChanged, onActiveChange: _vm.onActiveChanged}});
  }), 1)], 1);
};
var staticRenderFns = [];
var integrations_vue_vue_type_style_index_0_lang = "h2.ui-headline.integrations-headline {\n  font-family: var(--font);\n  color: var(--color-text);\n  margin: 0 0 var(--padding);\n  font-size: var(--font-size-l);\n  font-weight: 700;\n  padding-bottom: var(--padding-m);\n  border-bottom: 1px dashed var(--color-line-dashed);\n}";
const script = {
  data: () => ({
    count: 0,
    items: []
  }),
  components: {IntegrationItem},
  mounted() {
    this.load();
  },
  methods: {
    load() {
      IntegrationsApi.getTypes().then((res) => {
        this.items = res;
      });
    },
    onChanged() {
      this.load();
    },
    onActiveChanged(model) {
      model.isLoading = true;
      IntegrationsApi.saveActiveState({alias: model.type.alias, isActive: model.isActive}).then((res) => {
        if (!res.success) {
          model.isActive = !model.isActive;
          Notification.error("@integration.errors.couldnotupdatestate", res.errors[0].message);
        }
        model.isLoading = false;
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
component.options.__file = "app/pages/settings/integrations.vue";
var integrations = component.exports;
var integrations$1 = /* @__PURE__ */ Object.freeze({
  __proto__: null,
  [Symbol.toStringTag]: "Module",
  default: integrations
});
export {IntegrationsApi as I, integrations$1 as i};

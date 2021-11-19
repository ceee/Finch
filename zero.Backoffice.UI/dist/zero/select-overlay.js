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
import {n as normalizeComponent} from "./index.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-select-overlay"}, [_vm.loading ? _c("ui-loading") : _vm._e(), !_vm.loading ? _c("div", [_vm.list.length && _vm.configuration.parent ? _c("div", {staticClass: "ui-select-overlay-parent"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.title, expression: "title"}]}), _vm._v(": "), _c("strong", [_vm._v(_vm._s(_vm.configuration.parent.name))])]) : _vm._e(), _c("div", {staticClass: "ui-select-overlay-items"}, _vm._l(_vm.list, function(item) {
    return _c("button", {staticClass: "ui-select-overlay-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.onSelect(item);
    }}}, [_c("ui-icon", {staticClass: "ui-select-overlay-item-icon", attrs: {symbol: item[_vm.configuration.iconKey], size: 22}}), _c("span", {staticClass: "ui-select-overlay-item-text"}, [_c("ui-localize", {attrs: {value: item[_vm.configuration.labelKey]}}), item[_vm.configuration.descriptionKey] ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item[_vm.configuration.descriptionKey], expression: "item[configuration.descriptionKey]"}]}) : _vm._e()], 1)], 1);
  }), 0), !_vm.list.length ? _c("ui-message", {attrs: {type: "error", text: "@page.create.nonavailable"}}) : _vm._e()], 1) : _vm._e()], 1);
};
var staticRenderFns = [];
var selectOverlay_vue_vue_type_style_index_0_lang = ".ui-select-overlay {\n  text-align: left;\n}\n.ui-select-overlay .ui-message {\n  margin: 0;\n}\n.ui-select-overlay-parent {\n  margin: 30px 0 -10px 0;\n  border-radius: var(--radius);\n  /*border: 1px solid var(--color-line-light);*/\n  background: var(--color-box-nested);\n  line-height: 1.4;\n  color: var(--color-text-dim);\n  padding: 14px 16px;\n  font-size: var(--font-size);\n}\n.ui-select-overlay-parent strong {\n  color: var(--color-text);\n}\n.ui-select-overlay-items {\n  margin: 0 -16px;\n  max-height: 490px;\n  overflow-y: auto;\n}\n.ui-select-overlay-item {\n  display: grid;\n  width: 100%;\n  grid-template-columns: 70px 1fr auto;\n  gap: 12px;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n  padding: 16px;\n  border-radius: var(--radius);\n}\n.ui-select-overlay-item:hover, .ui-select-overlay-item:focus {\n  background: var(--color-tree-selected);\n}\n.ui-select-overlay-item + .ui-select-overlay-item {\n  margin-top: 2px;\n}\n.ui-select-overlay-item-text {\n  display: flex;\n  flex-direction: column;\n}\n.ui-select-overlay-item-text span {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.ui-select-overlay-item-icon {\n  color: var(--color-text);\n  width: 60px;\n  height: 60px;\n  padding: 20px;\n  border-radius: var(--radius);\n  background: var(--color-bg-shade-3);\n}";
const defaultConfig = {
  title: "@ui.create",
  labelKey: "name",
  descriptionKey: "description",
  iconKey: "icon",
  items: null
};
const script = {
  props: {
    config: Object
  },
  data: () => ({
    configuration: defaultConfig,
    list: [],
    loading: false,
    disabled: false
  }),
  mounted() {
    this.configuration = __assign(__assign({}, defaultConfig), this.config);
    this.loading = true;
    this.load();
  },
  methods: {
    load() {
      if (typeof this.configuration.items === "function") {
        this.configuration.items().then((res) => {
          this.list = res;
          this.loading = false;
        });
      } else {
        this.list = JSON.parse(JSON.stringify(this.configuration.items));
        this.loading = false;
      }
    },
    onSelect(item) {
      this.config.confirm(item);
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
component.options.__file = "app/components/overlays/select-overlay.vue";
var SelectOverlay = component.exports;
export {SelectOverlay as S};

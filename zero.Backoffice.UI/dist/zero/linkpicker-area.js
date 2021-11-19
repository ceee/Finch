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
import {g as get, n as normalizeComponent} from "./index.js";
import "./vendor.js";
var Api = {
  getListByQuery: async (query, config) => await get("storyLinks/getLinksByQuery", __assign(__assign({}, config), {params: {query}}))
};
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.area ? _c("div", {staticClass: "stories-linkpicker"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("ui-search", {model: {value: _vm.search, callback: function($$v) {
    _vm.search = $$v;
  }, expression: "search"}})], 1), _c("ui-property", {attrs: {vertical: true}}, [_c("div", {staticClass: "ui-list stories-linkpicker-items"}, _vm._l(_vm.list.items, function(item) {
    return _c("button", {key: item.id, staticClass: "ui-list-item has-icon", class: {"is-selected": _vm.selection.indexOf(item.id) > -1}, attrs: {type: "button"}, on: {click: function($event) {
      return _vm.onSelect(item);
    }}}, [_c("ui-icon", {staticClass: "ui-list-item-icon", attrs: {symbol: item.symbol, size: 18}}), _c("p", {staticClass: "ui-list-item-content"}, [_c("span", {staticClass: "ui-list-item-text"}, [_vm._v(_vm._s(item.name))]), _c("span", {staticClass: "ui-list-item-description"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.isActive ? "@ui.active" : "@ui.inactive", expression: "item.isActive ? '@ui.active' : '@ui.inactive'"}]}), item.area == "story" ? _c("span", [_vm._v(", "), _c("span", {directives: [{name: "date", rawName: "v-date", value: item.date, expression: "item.date"}]})]) : _vm._e()])]), _vm.selection.indexOf(item.id) > -1 ? _c("ui-icon", {staticClass: "ui-list-item-selected-icon", attrs: {symbol: "fth-check-circle", size: 16}}) : _vm._e()], 1);
  }), 0), _c("ui-pagination", {attrs: {pages: _vm.list.totalPages, page: _vm.page, inline: true}, on: {change: _vm.setPage}})], 1)], 1) : _vm._e();
};
var staticRenderFns = [];
var linkpickerArea_vue_vue_type_style_index_0_lang = ".stories-linkpicker-items {\n  margin: 0 -32px;\n}";
const script = {
  name: "storiesLinkpicker",
  props: {
    value: {
      type: Object,
      required: true
    },
    area: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    selection: [],
    search: null,
    debouncedSearch: null,
    list: {
      totalPages: 1,
      items: []
    },
    page: 1
  }),
  watch: {
    search() {
    },
    value() {
      this.selection = [this.value.values.id];
    }
  },
  mounted() {
    this.selection = this.value.values.id ? [this.value.values.id] : [];
  },
  created() {
    this.load();
  },
  methods: {
    load() {
      Api.getListByQuery({}).then((res) => {
        res.items.forEach((item) => {
          item.symbol = "fth-file-text";
          if (item.area === "tag")
            item.symbol = "fth-tag";
          else if (item.area === "author")
            item.symbol = "fth-user";
        });
        this.list = res;
      });
    },
    onSelect(item) {
      if (item) {
        this.selection = [item.id];
        this.value.values = {id: item.id, area: item.area};
      } else {
        this.selection = [];
        this.value.values = {id: null};
      }
      this.$emit("change", this.value);
      this.$emit("input", this.value);
    },
    setPage(index) {
      this.page = index;
      this.load();
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
component.options.__file = "../zero.Stories/Plugin/components/linkpicker-area.vue";
var linkpickerArea = component.exports;
export default linkpickerArea;

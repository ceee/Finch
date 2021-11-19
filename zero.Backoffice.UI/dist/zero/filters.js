import {n as normalizeComponent, Q as FiltersApi, A as Arrays} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-filters"}, [_c("ui-header-bar", {attrs: {title: "@shop.filter.list", count: _vm.count, "back-button": true}}), _c("div", {staticClass: "ui-blank-box"}, [_c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {handle: ".shop-filter-item-handle", onUpdate: _vm.onSortingUpdated}, expression: "{ handle: '.shop-filter-item-handle', onUpdate: onSortingUpdated }"}], staticClass: "shop-filters-list"}, _vm._l(_vm.items, function(item) {
    return _c("div", {key: item.id, staticClass: "ui-box shop-filter-item", class: {"is-light": !item.isActive}}, [_c("span", {staticClass: "shop-filter-item-handle"}, [_c("ui-icon", {attrs: {symbol: "fth-grip-vertical"}})], 1), _c("router-link", {staticClass: "shop-filter-item-name", attrs: {to: {name: "commerce-filters-edit", params: {id: item.id}}}}, [_c("strong", [_vm._v(_vm._s(item.name))]), _c("br"), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.filterType(item), expression: "filterType(item)"}]}), item.property ? _c("span", [_vm._v(": " + _vm._s(item.property))]) : _vm._e()]), _c("ui-toggle", {class: {onbg: !item.isActive}, attrs: {value: item.isActive, "content-left": true, "off-content": "@ui.disabled"}, on: {input: function($event) {
      return _vm.setActive(item, $event);
    }}})], 1);
  }), 0)])], 1);
};
var staticRenderFns = [];
var filters_vue_vue_type_style_index_0_lang = ".shop-filter-item {\n  margin: 0;\n  padding: 0 20px;\n  display: grid;\n  grid-template-columns: auto 1fr auto;\n  grid-gap: 20px;\n  align-items: center;\n  font-size: var(--font-size);\n}\n.shop-filter-item-handle {\n  cursor: grab;\n  align-self: stretch;\n  display: inline-flex;\n  align-items: center;\n  margin-top: -1px;\n}\n.shop-filter-item + .shop-filter-item {\n  margin-top: var(--padding-xxs);\n}\n.shop-filter-item-name {\n  margin: 0;\n  color: var(--color-text);\n  padding: 15px 0;\n}\n.shop-filter-item-name span {\n  display: inline-block;\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}";
const FILTER_TYPES = [
  {value: "@shop.filter.types.manufacturer", key: "manufacturer"},
  {value: "@shop.filter.types.price", key: "price"},
  {value: "@shop.filter.types.rating", key: "rating"},
  {value: "@shop.filter.types.property", key: "property"},
  {value: "@shop.filter.types.custom", key: "custom"}
];
const SORTING_TYPES = [
  {value: "@shop.filter.sorting_states.manual", key: "manual"},
  {value: "@shop.filter.sorting_states.numeric", key: "numeric"},
  {value: "@shop.filter.sorting_states.alphanumeric", key: "alphanumeric"},
  {value: "@shop.filter.sorting_states.relevance", key: "relevance"}
];
const script = {
  data: () => ({
    count: 0,
    items: []
  }),
  mounted() {
    FiltersApi.getAllWithCounts().then((response) => {
      this.items = response;
      this.count = response.length;
    });
  },
  methods: {
    filterType(item) {
      return FILTER_TYPES.find((t) => t.key == item.filterType).value;
    },
    sortingMethod(item) {
      return SORTING_TYPES.find((t) => t.key == item.sortingMethod).value;
    },
    onSortingUpdated(ev) {
      this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
      FiltersApi.saveSorting(this.items.map((x) => x.id)).then((res) => {
        console.info(res);
      });
    },
    setActive(item, isActive) {
      item.isActive = isActive;
      FiltersApi.setActive(item.id, isActive).then((res) => {
        if (!res.success) {
          item.isActive = !isActive;
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
component.options.__file = "../zero.Commerce/Plugin/pages/settings/filters.vue";
var filters = component.exports;
export default filters;

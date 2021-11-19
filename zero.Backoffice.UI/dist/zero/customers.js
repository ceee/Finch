import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-customers"}, [_c("ui-header-bar", {attrs: {title: "@shop.customer.list", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: "commerce-customers-create", decision: false}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "commerce.customers"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1);
};
var staticRenderFns = [];
var customers_vue_vue_type_style_index_0_lang = ".shop-customers .ui-table-cell[table-field=address] .ui-icon {\n  flex-shrink: 0;\n  margin-right: 10px;\n  position: relative;\n  top: -1px;\n}";
const script = {
  name: "shopCustomers",
  data: () => ({
    count: 0,
    filter: null
  })
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "../zero.Commerce/Plugin/pages/customers/customers.vue";
var customers = component.exports;
export default customers;

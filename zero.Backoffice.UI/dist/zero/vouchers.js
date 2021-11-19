import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-vouchers"}, [_c("ui-header-bar", {attrs: {title: "@laola.voucher.list", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: _vm.createRoute, decision: false}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "commerce.vouchers"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1);
};
var staticRenderFns = [];
var vouchers_vue_vue_type_style_index_0_lang = ".laola-vouchers .shop-orders-col-state2[data-state=expired], .laola-vouchers .shop-orders-col-state2[data-state=disabled], .laola-vouchers .shop-orders-col-state2[data-state=pending] {\n  font-weight: 400;\n  color: var(--color-text-dim);\n}";
const script = {
  data: () => ({
    count: 0,
    createRoute: "commerce-vouchers-create"
  })
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/commerce/vouchers.vue";
var vouchers = component.exports;
export default vouchers;

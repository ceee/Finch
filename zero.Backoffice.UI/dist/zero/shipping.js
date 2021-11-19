import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-shippingoptions"}, [_c("ui-header-bar", {attrs: {title: "@shop.shipping.list", prefix: "@shop.headline_prefix", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: "commerce-shipping-create"}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "commerce.shipping"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1);
};
var staticRenderFns = [];
const script = {
  data: () => ({
    count: 0
  })
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "../zero.Commerce/Plugin/pages/settings/shipping.vue";
var shipping = component.exports;
export default shipping;

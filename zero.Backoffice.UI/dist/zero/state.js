import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-state-button", {attrs: {items: _vm.config.items, value: _vm.value, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [String, Number]
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: Object
  },
  data: () => ({
    items: []
  })
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "app/editor/fields/state.vue";
var state = component.exports;
export default state;

import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("textarea", {staticClass: "ui-textarea", attrs: {rows: "3", disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: function($event) {
    return _vm.$emit("input", $event.target.value);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: String
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: Object
  }
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "app/editor/fields/textarea.vue";
var textarea = component.exports;
export default textarea;

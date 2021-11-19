import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-linkpicker", _vm._b({attrs: {config: _vm.config, value: _vm.value, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}}, "ui-linkpicker", {disabled: _vm.disabled, limit: _vm.limit, title: _vm.title, label: _vm.label, target: _vm.target, suffix: _vm.suffix, areas: _vm.areas}, false));
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [Object, Array],
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    limit: {
      type: Number,
      default: 1
    },
    title: {
      type: Boolean,
      default: true
    },
    label: {
      type: Boolean,
      default: false
    },
    target: {
      type: Boolean,
      default: true
    },
    suffix: {
      type: Boolean,
      default: false
    },
    areas: {
      type: Array,
      default: () => []
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
component.options.__file = "app/editor/fields/linkpicker.vue";
var linkpicker = component.exports;
export default linkpicker;

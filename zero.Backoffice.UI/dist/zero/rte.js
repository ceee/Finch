import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-rte", _vm._b({attrs: {value: _vm.value, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}}, "ui-rte", {maxLength: _vm.maxLength, placeholder: _vm.placeholder, setup: _vm.setup}, false));
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: String,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    maxLength: {
      type: Number,
      default: null
    },
    placeholder: {
      type: String,
      default: null
    },
    setup: {
      type: Function,
      default: () => {
      }
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
component.options.__file = "app/editor/fields/rte.vue";
var rte = component.exports;
export default rte;

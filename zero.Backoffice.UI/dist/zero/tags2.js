import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-tags", {attrs: {value: _vm.value, "max-items": _vm.limit, "max-length": _vm.maxItemLength, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: Array,
      default: () => []
    },
    disabled: Boolean,
    limit: {
      type: Number,
      default: 10
    },
    maxItemLength: {
      type: Number,
      default: 200
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
component.options.__file = "app/editor/fields/tags.vue";
var tags = component.exports;
export default tags;

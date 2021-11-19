import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-modules", {attrs: {value: _vm.value, config: _vm.config, tags: _vm.tags, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [String, Object, Array]
    },
    disabled: {
      type: Boolean,
      default: false
    },
    tags: {
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
component.options.__file = "app/editor/fields/modules.vue";
var modules = component.exports;
export default modules;

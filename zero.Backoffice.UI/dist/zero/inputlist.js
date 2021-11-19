import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-input-list", {attrs: {value: _vm.value, "add-label": _vm.addLabel, disabled: _vm.disabled, "max-items": _vm.limit, inline: true, "max-length": _vm.maxItemLength}, on: {input: function($event) {
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
    disabled: {
      type: Boolean,
      default: false
    },
    limit: {
      type: Number,
      default: 10
    },
    maxItemLength: {
      type: Number,
      default: 10
    },
    addLabel: {
      type: String,
      default: "@ui.add"
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
component.options.__file = "app/editor/fields/inputlist.vue";
var inputlist = component.exports;
export default inputlist;

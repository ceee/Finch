import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-check-list", {attrs: {value: _vm.value, reverse: _vm.reverse, items: _vm.items, limit: _vm.limit, "label-key": _vm.labelKey, "id-key": _vm.idKey, disabled: _vm.disabled}, on: {input: function($event) {
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
    items: {
      type: [Array, Function, Promise],
      required: true
    },
    limit: {
      type: Number,
      default: 100
    },
    reverse: Boolean,
    labelKey: {
      type: String,
      default: "value"
    },
    idKey: {
      type: String,
      default: "key"
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
component.options.__file = "app/editor/fields/checklist.vue";
var checklist = component.exports;
export default checklist;

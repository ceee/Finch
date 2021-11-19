import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-alias", {attrs: {value: _vm.value, name: _vm.entity.name, "max-length": _vm.maxLength, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: String,
      default: null
    },
    entity: {
      type: Object,
      required: true
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    maxLength() {
      return this.config.maxLength > 0 ? this.config.maxLength : null;
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
component.options.__file = "app/editor/fields/alias.vue";
var alias = component.exports;
export default alias;

import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-box"}, [_c("input", {directives: [{name: "placeholder", rawName: "v-placeholder", value: {placeholder: _vm.placeholder, model: _vm.entity}, expression: "{ placeholder, model: entity }"}], staticClass: "ui-input", attrs: {type: "text", disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: function($event) {
    return _vm.$emit("input", +$event.target.value);
  }}})]);
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: Number,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    placeholder: String,
    entity: Object
  }
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "app/editor/fields/currency.vue";
var currency = component.exports;
export default currency;

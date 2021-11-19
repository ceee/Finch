import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("input", {directives: [{name: "placeholder", rawName: "v-placeholder", value: {placeholder: _vm.placeholder, model: _vm.entity}, expression: "{ placeholder, model: entity }"}], staticClass: "ui-input", attrs: {type: "text", maxlength: _vm.maxLength, disabled: _vm.disabled}, domProps: {value: _vm.value}, on: {input: function($event) {
    return _vm.onChange($event.target.value);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: Number,
    maxLength: Number,
    placeholder: [String, Function],
    disabled: Boolean,
    entity: Object
  },
  methods: {
    onChange(value) {
      var parsedValue = parseFloat(value);
      if (isNaN(parsedValue)) {
        return;
      }
      this.$emit("input", parsedValue);
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
component.options.__file = "app/editor/fields/number.vue";
var number = component.exports;
export default number;

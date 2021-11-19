import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-box"}, [_c("ui-datepicker", _vm._b({attrs: {value: _vm.value, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}}, "ui-datepicker", {format: _vm.format, time: _vm.time, maxDate: _vm.maxDate, minDate: _vm.minDate, amPm: _vm.amPm}, false))], 1);
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [String, Date],
      default: {
        from: null,
        to: null
      }
    },
    format: {
      type: String,
      default: null
    },
    time: {
      type: Boolean,
      default: false
    },
    maxDate: {
      type: [String, Date],
      default: null
    },
    minDate: {
      type: [String, Date],
      default: null
    },
    amPm: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: Boolean,
      default: false
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
component.options.__file = "app/editor/fields/datepicker.vue";
var datepicker = component.exports;
export default datepicker;

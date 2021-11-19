import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-mediapicker", {attrs: {config: {limit: _vm.limit, disallowSelect: _vm.disallowSelect, disallowUpload: _vm.disallowUpload, fileExtensions: _vm.fileExtensions, maxFileSize: _vm.maxFileSize}, value: _vm.value, disabled: _vm.disabled}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}});
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [String, Array],
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
    disallowSelect: {
      type: Boolean,
      default: false
    },
    disallowUpload: {
      type: Boolean,
      default: false
    },
    fileExtensions: {
      type: Array,
      default: null
    },
    maxFileSize: {
      type: Number,
      default: 10
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
component.options.__file = "app/editor/fields/media.vue";
var media = component.exports;
export default media;

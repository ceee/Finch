import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-linkpicker-area-media"}, [_vm._v(" media ")]);
};
var staticRenderFns = [];
var media_vue_vue_type_style_index_0_lang = "";
const script = {
  name: "uiLinkpickerAreaMedia",
  props: {
    value: {
      type: [Object, Array],
      default: null
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
component.options.__file = "app/components/pickers/linkPicker/areas/media.vue";
var media = component.exports;
export default media;

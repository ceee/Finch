import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "stories-authors"}, [_c("ui-header-bar", {attrs: {title: "@stories.author.list", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: _vm.createRoute, decision: false}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "stories.authors"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1);
};
var staticRenderFns = [];
var authors_vue_vue_type_style_index_0_lang = ".stories-authors .ui-table-field-image {\n  border-radius: 50px;\n}\n.stories-authors .ui-table-cell[table-field=avatarId] {\n  padding: 12px;\n  padding-left: 20px;\n}\n.stories-authors .ui-table-cell[table-field=name] {\n  border-left-color: transparent;\n}";
const script = {
  data: () => ({
    count: 0,
    createRoute: "stories-authors-create"
  })
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "../zero.Stories/Plugin/pages/authors.vue";
var authors = component.exports;
export default authors;

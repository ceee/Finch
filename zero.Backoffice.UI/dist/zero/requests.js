import {n as normalizeComponent, a4 as list, a5 as RequestsApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-requests"}, [_c("ui-header-bar", {attrs: {title: "@laola.request.list", prefix: "@shop.headline_prefix", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _vm.$refs.table && !_vm.$refs.table.query.search ? _c("div", [_c("label", {staticClass: "ui-native-check onbg"}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.showDone, expression: "showDone"}], attrs: {type: "checkbox"}, domProps: {checked: Array.isArray(_vm.showDone) ? _vm._i(_vm.showDone, null) > -1 : _vm.showDone}, on: {change: function($event) {
    var $$a = _vm.showDone, $$el = $event.target, $$c = $$el.checked ? true : false;
    if (Array.isArray($$a)) {
      var $$v = null, $$i = _vm._i($$a, $$v);
      if ($$el.checked) {
        $$i < 0 && (_vm.showDone = $$a.concat([$$v]));
      } else {
        $$i > -1 && (_vm.showDone = $$a.slice(0, $$i).concat($$a.slice($$i + 1)));
      }
    } else {
      _vm.showDone = $$c;
    }
  }}}), _c("span", {staticClass: "ui-native-check-toggle"}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@laola.request.showDone", expression: "'@laola.request.showDone'"}]})])]) : _vm._e()], 1), _vm.isLoaded ? _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: _vm.list}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1) : _vm._e()], 1);
};
var staticRenderFns = [];
var requests_vue_vue_type_style_index_0_lang = ".laola-requests .ui-icon.-flag {\n  position: relative;\n  top: 4px;\n  margin-right: 3px;\n  margin-left: -2px;\n  display: inline-block;\n  transform: scale(0.8);\n  transform-origin: 50% 0%;\n}\n.laola-requests .ui-native-check {\n  margin-left: var(--padding-s);\n}\n.laola-requests-offer {\n  display: inline-grid;\n  grid-template-columns: auto 1fr;\n  grid-gap: var(--padding-xs);\n  align-items: center;\n}\n.laola-requests-offer .-image {\n  border-radius: 50px;\n  max-height: 32px;\n  max-width: 32px;\n  background: var(--color-box-nested);\n}\n.laola-requests-offer .-image.-empty {\n  height: 32px;\n  width: 32px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  color: var(--color-text-dim-one);\n}";
const KEY_SHOW_DONE = "laola.ui-table-requests.show-done";
const script = {
  data: () => ({
    count: 0,
    createRoute: "commerce-requests-create",
    showDone: false,
    isLoaded: false,
    list
  }),
  watch: {
    $route(to, from) {
      this.rebuild();
    },
    showDone(done) {
      if (this.isLoaded) {
        localStorage.setItem(KEY_SHOW_DONE, JSON.stringify(done));
        this.$refs.table.update();
      }
    }
  },
  created() {
    let storedShowDone = localStorage.getItem(KEY_SHOW_DONE);
    if (storedShowDone !== null) {
      this.showDone = JSON.parse(storedShowDone);
    }
    this.rebuild();
    this.isLoaded = true;
  },
  methods: {
    rebuild() {
      this.list.onFetch((filter) => RequestsApi.getListByQuery(filter, this.showDone));
      if (this.$refs.table) {
        this.$refs.table.initialize();
      }
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
component.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/requests/requests.vue";
var requests = component.exports;
export default requests;

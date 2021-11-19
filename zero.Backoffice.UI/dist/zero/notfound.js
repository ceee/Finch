import {n as normalizeComponent} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "page page-error"}, [_c("ui-icon", {staticClass: "page-error-icon", attrs: {symbol: "fth-cloud-snow", size: 82}}), _c("p", {staticClass: "page-error-text"}, [_c("strong", {staticClass: "page-error-headline"}, [_vm._v("Not found")]), _c("br"), _vm._v(" The requested resource could not be found "), _c("br"), _c("code", [_vm._v(_vm._s(_vm.path))])]), _c("ui-button", {staticClass: "page-error-button", attrs: {type: "light onbg", label: _vm.detailsText}, on: {click: function($event) {
    _vm.details = !_vm.details;
  }}}), _vm.details ? [_c("br"), _c("br"), _c("div", {staticClass: "page-error-routes"}, [_c("span", [_vm._v("#")]), _c("span", [_vm._v("Name")]), _c("span", [_vm._v("Path")]), _vm._l(_vm.routes, function(route, index) {
    return [_c("span", [_vm._v(_vm._s(index + 1) + ".")]), _c("b", [_vm._v(_vm._s(route.name))]), _c("span", [_vm._v(_vm._s(route.path))])];
  })], 2)] : _vm._e()], 2);
};
var staticRenderFns = [];
var notfound_vue_vue_type_style_index_0_lang = ".page-error {\n  width: 100%;\n  min-height: 100%;\n  display: flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: center;\n  color: var(--color-text);\n  text-align: center;\n  padding: var(--padding);\n  overflow-y: auto;\n}\n.page-error-icon {\n  color: var(--color-text);\n  margin-bottom: 20px;\n}\n.page-error-text {\n  font-size: var(--font-size);\n  color: var(--color-text-dim);\n  line-height: 1.4em;\n}\n.page-error-headline {\n  display: inline-block;\n  margin-bottom: 10px;\n  font-size: var(--font-size-l);\n  color: var(--color-text);\n}\n.page-error-button {\n  margin-top: 10px;\n}\n.page-error-routes {\n  display: grid;\n  grid-template-columns: auto auto 1fr;\n  width: 100%;\n  max-width: 100%;\n  text-align: left;\n  border-top: 1px solid var(--color-line-onbg);\n  border-left: 1px solid var(--color-line-onbg);\n  margin-top: 30px;\n}\n.page-error-routes span, .page-error-routes b {\n  border: 1px solid var(--color-line-onbg);\n  border-left: none;\n  border-top: none;\n  padding: 8px 10px 6px;\n  font-family: Consolas;\n  font-size: 12px;\n}\n.page-error-routes span:nth-child(3n+1), .page-error-routes b:nth-child(3n+1) {\n  color: var(--color-text-dim);\n}";
const script = {
  data: () => ({
    path: null,
    routes: [],
    details: false
  }),
  watch: {
    $route: function(val) {
      this.rebuild();
    }
  },
  computed: {
    detailsText() {
      return this.details ? "Hide" : "Defined routes";
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.path = this.$router.options.base + this.$route.path.substring(1);
      this.routes = [];
      this.$router.options.routes.forEach((route) => {
        this.routes.push({
          path: route.path,
          name: route.name
        });
        if (route.children) {
          route.children.forEach((child) => {
            this.routes.push({
              path: route.path + "/" + child.path,
              name: child.name
            });
          });
        }
      });
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
component.options.__file = "app/pages/notfound.vue";
var notfound = component.exports;
export default notfound;

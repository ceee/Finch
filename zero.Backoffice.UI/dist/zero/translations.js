import {n as normalizeComponent, O as Overlay, i as __vitePreload} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "translations"}, [_c("ui-header-bar", {attrs: {title: "@translation.list", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.table}}), _c("ui-add-button", {attrs: {route: _vm.createRoute}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", attrs: {config: "translations"}, on: {count: function($event) {
    _vm.count = $event;
  }}})], 1)], 1);
};
var staticRenderFns = [];
const script = {
  data: () => ({
    count: 0,
    createRoute: zero.alias.settings.translations + "-create"
  }),
  watch: {
    $route: function(route) {
      this.handleRouteChange();
    }
  },
  created() {
    this.handleRouteChange();
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    },
    handleRouteChange() {
      if (this.$route.params.id) {
        this.edit(this.$route.params.id);
      } else if (this.$route.name === this.createRoute) {
        this.edit();
      } else {
        Overlay.close();
      }
    },
    edit(id) {
      Overlay.open({
        component: () => __vitePreload(() => __import__("./translation.js"), true ? ["/zero/translation.js","/zero/translation.css","/zero/index.js","/zero/index.css","/zero/vendor.js"] : void 0),
        width: 700,
        model: {id}
      }).then((res) => {
        this.$router.go(-1);
      }, () => {
        this.$router.go(-1);
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
component.options.__file = "app/pages/settings/translations.vue";
var translations = component.exports;
export default translations;

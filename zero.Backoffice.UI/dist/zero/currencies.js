import {n as normalizeComponent, H as CurrenciesApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "currencies"}, [_c("ui-header-bar", {attrs: {title: "@shop.currency.list", prefix: "@shop.headline_prefix", count: _vm.count, "back-button": true}}, [_c("ui-button", {attrs: {type: "primary", label: "@ui.add", icon: "fth-plus"}, on: {click: _vm.add}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("div", {staticClass: "currencies-items"}, _vm._l(_vm.items, function(item) {
    return _c("router-link", {key: item.id, staticClass: "currencies-item", attrs: {to: _vm.getLink(item)}}, [_c("b", {staticClass: "currencies-item-symbol"}, [_vm._v(_vm._s(item.symbol))]), _c("strong", {staticClass: "currencies-item-name"}, [_vm._v(_vm._s(item.name))]), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.isDefault ? "@shop.currency.default" : "@shop.currency.option", expression: "item.isDefault ? '@shop.currency.default' : '@shop.currency.option'"}], staticClass: "currencies-item-status"})]);
  }), 1)])], 1);
};
var staticRenderFns = [];
var currencies_vue_vue_type_style_index_0_lang = ".currencies-items {\n  display: grid;\n  gap: var(--padding);\n  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));\n  align-items: stretch;\n}\na.currencies-item {\n  display: flex;\n  flex-direction: column;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  padding: var(--padding-m);\n  text-align: center;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  box-shadow: var(--shadow-short);\n}\n.currencies-item-symbol {\n  font-size: 32px;\n  margin-bottom: 10px;\n}\n.currencies-item-status {\n  align-self: center;\n  display: inline-block;\n  font-size: 9px;\n  font-weight: 700;\n  margin-top: 15px;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n}";
const script = {
  data: () => ({
    count: 0,
    items: []
  }),
  components: {},
  mounted() {
    CurrenciesApi.getAll().then((response) => {
      this.items = response;
      this.count = response.length;
    });
  },
  methods: {
    add() {
      this.$router.push({name: this.$route.name + "-create"});
    },
    getLink(item) {
      return {
        name: this.$route.name + "-edit",
        params: {id: item.id}
      };
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
component.options.__file = "../zero.Commerce/Plugin/pages/settings/currencies.vue";
var currencies = component.exports;
export default currencies;

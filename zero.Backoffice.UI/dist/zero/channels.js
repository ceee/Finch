import {n as normalizeComponent, b as MediaApi, t as ChannelsApi, O as Overlay} from "./index.js";
import {S as SelectOverlay} from "./select-overlay.js";
import "./vendor.js";
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("router-link", {staticClass: "channels-item", attrs: {to: _vm.link}}, [_c("strong", {staticClass: "channels-item-name"}, [_vm._v(" " + _vm._s(_vm.value.name) + " "), _vm.image ? _c("img", {staticClass: "channels-item-image", attrs: {src: _vm.image}}) : _vm._e()]), _c("span", {staticClass: "channels-item-line"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.channel.lastOrderDate", expression: "'@shop.channel.lastOrderDate'"}]}), _c("span", {directives: [{name: "date", rawName: "v-date", value: _vm.value.lastOrderDate, expression: "value.lastOrderDate"}]})]), _c("span", {staticClass: "channels-item-line"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.channel.turnover", expression: "'@shop.channel.turnover'"}]}), _c("span", {directives: [{name: "currency", rawName: "v-currency", value: _vm.value.turnover, expression: "value.turnover"}]})]), _c("span", {staticClass: "channels-item-line"}, [_c("span", [_vm._v("State")]), _c("span", [_vm.value.isDefault ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@ui.default", expression: "'@ui.default'"}], staticClass: "channels-item-status is-active"}) : _vm._e(), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: _vm.value.isActive ? "@ui.active" : "@ui.inactive", expression: "value.isActive ? '@ui.active' : '@ui.inactive'"}], staticClass: "channels-item-status"})])])]);
};
var staticRenderFns$1 = [];
var item_vue_vue_type_style_index_0_lang = "a.channels-item {\n  display: flex;\n  flex-direction: column;\n  justify-content: flex-start;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  padding: var(--padding-m);\n  text-align: left;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  box-shadow: var(--shadow-short);\n}\n.channels-item-image {\n  max-width: 60px;\n  max-height: 34px;\n  margin-right: -2px;\n}\n.channels-item-name {\n  display: flex;\n  justify-content: space-between;\n  min-height: 40px;\n  align-items: center;\n  margin-top: -10px;\n  margin-bottom: 15px;\n}\n.channels-item-line {\n  color: var(--color-text-dim);\n  display: flex;\n  justify-content: space-between;\n  min-height: 23px;\n}\n.channels-item-line + .channels-item-line {\n  margin-top: 5px;\n}\n.channels-item-status {\n  display: inline-block;\n  font-size: 9px;\n  font-weight: 700;\n  text-transform: uppercase;\n  background: var(--color-box-nested);\n  color: var(--color-text);\n  height: 22px;\n  line-height: 22px;\n  padding: 0 10px;\n  border-radius: 16px;\n  letter-spacing: 0.5px;\n  margin-right: -2px;\n}\n.channels-item-status + .channels-item-status {\n  margin-left: 5px;\n}";
const script$1 = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    }
  },
  computed: {
    link() {
      return {
        name: "commerce-channels-edit",
        params: {id: this.value.id}
      };
    },
    image() {
      return this.value.image != null ? MediaApi.getImageSource(this.value.image) : null;
    }
  }
};
const __cssModules$1 = {};
var component$1 = normalizeComponent(script$1, render$1, staticRenderFns$1, false, injectStyles$1, null, null, null);
function injectStyles$1(context) {
  for (let o in __cssModules$1) {
    this[o] = __cssModules$1[o];
  }
}
component$1.options.__file = "../zero.Commerce/Plugin/pages/channels/partials/item.vue";
var ChannelItem = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "languages"}, [_c("ui-header-bar", {attrs: {title: "@shop.channel.list", count: _vm.count}}, [_c("ui-search", {staticClass: "onbg", model: {value: _vm.gridConfig.search, callback: function($$v) {
    _vm.$set(_vm.gridConfig, "search", $$v);
  }, expression: "gridConfig.search"}}), _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", icon: "fth-more-horizontal"}})];
  }, proxy: true}])}, [_c("ui-dropdown-button", {attrs: {label: "@ui.export.action", icon: "fth-share"}})], 1), _c("ui-add-button", {attrs: {decision: false}, on: {click: _vm.create}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-datagrid", {on: {count: function($event) {
    _vm.count = $event;
  }}, model: {value: _vm.gridConfig, callback: function($$v) {
    _vm.gridConfig = $$v;
  }, expression: "gridConfig"}})], 1)], 1);
};
var staticRenderFns = [];
const script = {
  data: () => ({
    count: 0,
    gridConfig: {},
    actions: []
  }),
  created() {
    this.gridConfig = {
      width: 360,
      component: ChannelItem,
      items: ChannelsApi.getByQuery,
      search: null
    };
  },
  methods: {
    create() {
      Overlay.open({
        component: SelectOverlay,
        width: 480,
        theme: "dark",
        items: ChannelsApi.getChannelTypes
      }).then((item) => {
        this.$router.push({
          name: "commerce-channels-create",
          params: {type: item.alias}
        });
      }, () => {
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
component.options.__file = "../zero.Commerce/Plugin/pages/channels/channels.vue";
var channels = component.exports;
export default channels;

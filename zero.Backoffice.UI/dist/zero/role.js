import {n as normalizeComponent, k as UserRolesApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "role", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {prefix: "@role.roles", title: "@role.name", disabled: _vm.disabled, "is-create": !_vm.$route.params.id, state: form.state, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "userRole", meta: _vm.meta, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})];
  }}])});
};
var staticRenderFns = [];
var role_vue_vue_type_style_index_0_lang = ".role-permission-toggle + .role-permission-toggle {\n  padding-top: 0;\n  margin-top: var(--padding);\n}\n.role .ui-box + .ui-permissions {\n  margin-top: var(--padding);\n}";
const script = {
  data: () => ({
    meta: {},
    model: {name: null, features: [], domains: [], claims: []},
    route: zero.alias.settings.users + "-role",
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(!this.$route.params.id ? UserRolesApi.getEmpty() : UserRolesApi.getById(this.$route.params.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
      });
    },
    onSubmit(form) {
      form.handle(UserRolesApi.save(this.model));
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(UserRolesApi.delete.bind(this, this.$route.params.id));
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
component.options.__file = "app/pages/settings/role.vue";
var role = component.exports;
export default role;

import {n as normalizeComponent, l as UsersApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "user", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {prefix: "@user.users", title: "@user.name", disabled: _vm.disabled, "is-create": !_vm.$route.params.id, state: form.state, "can-delete": _vm.meta.canDelete}, on: {delete: _vm.onDelete}, scopedSlots: _vm._u([{key: "actions", fn: function() {
      return [_vm.model.isActive ? _c("ui-dropdown-button", {attrs: {label: "@ui.disable", icon: "fth-minus-circle", disabled: _vm.disabled}, on: {click: _vm.onActiveChange}}) : _vm._e(), !_vm.model.isActive ? _c("ui-dropdown-button", {attrs: {label: "@ui.enable", icon: "fth-plus-circle", disabled: _vm.disabled}, on: {click: _vm.onActiveChange}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "@user.changePassword", icon: "fth-lock", disabled: _vm.disabled}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "user", meta: _vm.meta, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "settings", fn: function() {
      return [_c("ui-property", {staticClass: "is-toggle", attrs: {label: "@user.fields.isLockedOut", "is-text": true}}, [_c("ui-toggle", {attrs: {value: _vm.isLockedOut, negative: true, disabled: _vm.disabled}, on: {input: _vm.onLockoutChange}})], 1), _vm.isLockedOut ? _c("p", {staticClass: "ui-message type-error block user-aside-error"}, [_c("i", {staticClass: "ui-message-icon fth-alert-circle"}), _c("span", {staticClass: "ui-message-text"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize:html", value: "@user.fields.isLockedOut_warning", expression: "'@user.fields.isLockedOut_warning'", arg: "html"}]}), _vm._v(":"), _c("br"), _c("strong", [_c("ui-date", {attrs: {format: "long"}, model: {value: _vm.model.lockoutEnd, callback: function($$v) {
        _vm.$set(_vm.model, "lockoutEnd", $$v);
      }, expression: "model.lockoutEnd"}})], 1)])]) : _vm._e(), !_vm.model.isActive ? _c("ui-message", {staticClass: "user-aside-error", attrs: {type: "error", text: "@user.fields.isDisabled_warning"}}) : _vm._e()];
    }, proxy: true}, {key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})];
  }}])});
};
var staticRenderFns = [];
var user_vue_vue_type_style_index_0_lang = ".user-aside-error {\n  margin-bottom: 0;\n}";
const script = {
  data: () => ({
    meta: {},
    model: {
      avatarId: null,
      name: null,
      email: null,
      lockoutEnd: null
    },
    route: zero.alias.settings.users + "-edit",
    disabled: false,
    originalLockoutEnd: null
  }),
  computed: {
    isLockedOut() {
      return !!this.model.lockoutEnd;
    }
  },
  methods: {
    onLoad(form) {
      form.load(!this.$route.params.id ? UsersApi.getEmpty() : UsersApi.getById(this.$route.params.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
        this.originalLockoutEnd = this.model.lockoutEnd;
      });
    },
    onSubmit(form) {
      form.handle(UsersApi.save(this.model)).then((res) => {
        if (res.entity.id === AuthApi.user.id) {
          AuthApi.setUser(res.entity);
        }
      });
    },
    onDelete(item, opts) {
      opts.hide();
      this.$refs.form.onDelete(UsersApi.delete.bind(this, this.$route.params.id));
    },
    onLockoutChange(locked) {
      this.model.lockoutEnd = locked ? this.originalLockoutEnd : null;
    },
    onActiveChange(value, opts) {
      opts.loading(true);
      const isActive = !this.model.isActive;
      let promise = isActive ? UsersApi.enable(this.model) : UsersApi.disable(this.model);
      promise.then((response) => {
        opts.loading(false);
        opts.hide();
        if (response.success) {
          this.model.isActive = isActive;
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
component.options.__file = "app/pages/settings/user.vue";
var user = component.exports;
export default user;

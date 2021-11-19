import {n as normalizeComponent, k as UserRolesApi} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "users"}, [_c("ui-header-bar", {attrs: {title: "@user.title", "back-button": true}}, [_c("ui-table-filter", {attrs: {attach: _vm.$refs.usersTable}}), _c("ui-add-button", {attrs: {type: "light onbg", route: _vm.createRoleRoute, label: "@user.add_role"}}), _c("ui-button", {attrs: {type: "primary", label: "@user.add_user"}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@role.roles", expression: "'@role.roles'"}], staticClass: "ui-headline users-group-headline"}), _c("div", {staticClass: "users-roles"}, _vm._l(_vm.roles, function(role) {
    return _c("router-link", {key: role.id, staticClass: "users-role", attrs: {to: _vm.getRoleLink(role)}}, [_c("ui-icon", {staticClass: "users-role-icon", attrs: {symbol: role.icon, size: 24}}), _c("strong", [_vm._v(_vm._s(role.name))]), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: role.countClaims !== 1 ? "@user.count_permissions" : "@user.one_permission", tokens: {count: role.claims.length}}, expression: "{ key: role.countClaims !== 1 ? '@user.count_permissions' : '@user.one_permission', tokens: { count: role.claims.length }}"}], staticClass: "users-role-minor"})], 1);
  }), 1)]), _c("div", {staticClass: "ui-blank-box"}, [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "@user.users", expression: "'@user.users'"}], staticClass: "ui-headline users-group-headline"}), _c("ui-table", {ref: "usersTable", attrs: {config: "users"}})], 1)], 1);
};
var staticRenderFns = [];
var users_vue_vue_type_style_index_0_lang = "h2.users-group-headline {\n  margin-bottom: 30px !important;\n}\n.users .ui-table-field-image {\n  border-radius: 50px;\n}\n.users .ui-table-cell[table-field=avatar] {\n  padding: 12px;\n  padding-left: 20px;\n}\n.users .ui-table-cell[table-field=name] {\n  border-left-color: transparent;\n}\n.users-roles {\n  display: grid;\n  gap: var(--padding);\n  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));\n  align-items: stretch;\n  margin-bottom: calc(var(--padding) * 2);\n}\na.users-role {\n  display: flex;\n  flex-direction: column;\n  background: var(--color-box);\n  border-radius: var(--radius);\n  padding: var(--padding-m) var(--padding);\n  text-align: center;\n  color: var(--color-text);\n  font-size: var(--font-size);\n  line-height: 1.5;\n  box-shadow: var(--shadow-short);\n}\n.users-role-minor {\n  color: var(--color-text-dim);\n}\n.users-role-icon {\n  text-align: center;\n  display: inline-block;\n  margin: 0 auto var(--padding-m);\n  position: relative;\n}";
const script = {
  data: () => ({
    createRoleRoute: "roles-create",
    roles: []
  }),
  created() {
    UserRolesApi.getAll().then((items) => {
      this.roles = items;
    });
  },
  methods: {
    getRoleLink(item) {
      return {
        name: "roles-edit",
        params: {id: item.id},
        query: {scope: "shared "}
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
component.options.__file = "app/pages/settings/users.vue";
var users = component.exports;
export default users;

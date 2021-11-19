import {n as normalizeComponent, U as UiEditor, h as hub, s as CategoriesApi} from "./index.js";
import {c as find} from "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-category", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-form-header", {attrs: {title: "@shop.category.name", disabled: _vm.disabled, "is-create": !_vm.id, state: form.state, "can-delete": _vm.meta.canDelete, "active-toggle": true}, on: {delete: _vm.onDelete}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _c("ui-editor", {attrs: {config: "commerce.category", meta: _vm.meta, disabled: _vm.disabled}, scopedSlots: _vm._u([{key: "below", fn: function() {
      return [_c("ui-editor-infos", {attrs: {disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
        _vm.model = $$v;
      }, expression: "model"}})];
    }, proxy: true}], null, true), model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})];
  }}])});
};
var staticRenderFns = [];
const script = {
  props: ["id", "channelId"],
  components: {UiEditor},
  data: () => ({
    meta: {},
    model: {name: null},
    route: "commerce-categories-edit",
    disabled: false
  }),
  mounted() {
    hub.$on("shop.category.sort", (items) => {
      let item = find(items, (x) => x.id === this.id);
      if (item) {
        this.model.sort = item.sort;
      }
    });
    hub.$on("shop.category.move", (item) => {
      if (item.id === this.id) {
        this.model.parentId = item.parentId;
      }
    });
    hub.$on("shop.category.delete", (ids) => {
      if (ids.indexOf(this.id) > -1) {
        ids.indexOf(this.model.parentId) > -1 ? null : this.model.parentId;
        this.$router.replace({
          name: "commerce-categories",
          params: {channelId: this.model.channelId}
        });
      }
    });
  },
  methods: {
    onLoad(form) {
      form.load(!this.id ? CategoriesApi.getEmpty() : CategoriesApi.getById(this.id)).then((response) => {
        if (!this.channelId) {
          let channelId = response.entity.channelId;
          if (!channelId) {
            if (response.entity.appId === zero.sharedAppId) {
              channelId = zero.sharedAppId;
            } else {
              channelId = zero.commerce.alias.localChannel;
            }
          }
          this.$router.replace({
            name: "commerce-catalogue-channel-category",
            params: {channelId, categoryId: this.id}
          });
        }
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
        this.model.connection = response.entity.connection || {categoryId: null};
      });
    },
    onSubmit(form) {
      form.handle(CategoriesApi.save(this.model));
    },
    onDelete(item, opts) {
      this.$refs.form.onDelete(CategoriesApi.delete.bind(this, this.id));
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
component.options.__file = "../zero.Commerce/Plugin/pages/categories/category.vue";
var category = component.exports;
export default category;

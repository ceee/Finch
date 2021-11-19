import {g as get, d as del, n as normalizeComponent, a as PagesApi, O as Overlay, h as hub} from "./index.js";
import "./vendor.js";
const base = "recycleBin/";
var RecycleBinApi = {
  getByQuery: async (query) => await get(base + "getByQuery", {params: query}),
  getCountByOperation: async (operationId) => await get(base + "getCountByOperation", {params: {operationId}}),
  delete: async (id) => await del(base + "delete", {params: {id}}),
  deleteByGroup: async (group) => await del(base + "deleteByGroup", {params: {group}})
};
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "pages-recyclebin-actions", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: _vm.model.name, "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}})];
  }, proxy: true}])}, [_c("button", {staticClass: "pages-recyclebin-action", on: {click: function($event) {
    return _vm.restore(false);
  }}}, [_c("i", {staticClass: "pages-recyclebin-action-icon fth-rotate-ccw"}), _c("p", {staticClass: "pages-recyclebin-action-text"}, [_c("strong", [_vm._v("Restore page")]), _c("span", [_vm._v("Moves this page back into the page tree.")])])]), _vm.model.operationId ? _c("button", {staticClass: "pages-recyclebin-action", on: {click: function($event) {
    return _vm.restore(true);
  }}}, [_c("i", {staticClass: "pages-recyclebin-action-icon fth-zap"}), _c("p", {staticClass: "pages-recyclebin-action-text"}, [_c("strong", [_vm._v("Undo operation")]), _c("span", [_vm._v("Restores all " + _vm._s(_vm.operationCount) + " pages from the affected operation.")])])]) : _vm._e(), _c("button", {staticClass: "pages-recyclebin-action"}, [_c("i", {staticClass: "pages-recyclebin-action-icon fth-trash is-negative"}), _c("p", {staticClass: "pages-recyclebin-action-text"}, [_c("strong", [_vm._v("Delete page...")]), _c("span", [_vm._v("Remove this recycled page forever.")])])])]);
};
var staticRenderFns$1 = [];
var recyclebinActions_vue_vue_type_style_index_0_lang = ".pages-recyclebin-action {\n  display: grid;\n  grid-template-columns: auto 1fr;\n  gap: var(--padding);\n  align-items: center;\n  width: 100%;\n  background: var(--color-box);\n  box-shadow: var(--shadow-short);\n  border-radius: var(--radius);\n  padding: 16px;\n}\n.pages-recyclebin-action + .pages-recyclebin-action {\n  margin-top: 16px;\n}\n.pages-recyclebin-action-text {\n  margin: 16px 0;\n  line-height: 1.5;\n}\n.pages-recyclebin-action-text strong {\n  display: block;\n  margin-bottom: 5px;\n}\n.pages-recyclebin-action-text span {\n  color: var(--color-text-dim);\n}\n.pages-recyclebin-action-icon {\n  font-size: 18px;\n  margin-right: -6px;\n  align-self: stretch;\n  background: var(--color-box-nested);\n  padding: 10px;\n  width: 50px;\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  border-radius: var(--radius);\n  color: var(--color-text);\n}\n.pages-recyclebin-action-icon.is-negative {\n  color: var(--color-accent-error);\n}\n.pages-recyclebin-actions content {\n  padding-top: 0;\n}";
const script$1 = {
  props: {
    isCopy: {
      type: Boolean,
      default: false
    },
    model: Object,
    config: Object
  },
  data: () => ({
    state: "default",
    operationCount: 0
  }),
  mounted() {
    RecycleBinApi.getCountByOperation(this.model.operationId).then((count) => {
      this.operationCount = count;
    });
  },
  methods: {
    restore(includeDescendants) {
      this.state = "loading";
      PagesApi.restore(this.model.id, includeDescendants).then((res) => {
        if (res.success) {
          this.state = "success";
          this.config.confirm(res.model);
        } else {
          this.state = "error";
        }
      });
    },
    delete() {
      this.state = "loading";
      RecycleBinApi.delete(this.model.id).then((res) => {
        if (res.success) {
          this.state = "success";
          res.model.deleted = true;
          this.config.confirm(res.model);
        } else {
          this.state = "error";
        }
      });
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
component$1.options.__file = "app/pages/pages/recyclebin/recyclebin-actions.vue";
var RecycleBinActionsOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "pages-recyclebin"}, [_c("ui-header-bar", {attrs: {title: "@recyclebin.name", count: _vm.count, "back-button": true}}, [_c("ui-table-filter", {model: {value: _vm.tableConfig, callback: function($$v) {
    _vm.tableConfig = $$v;
  }, expression: "tableConfig"}}), _c("ui-button", {attrs: {type: "light onbg", label: "@recyclebin.purge"}, on: {click: _vm.purge}})], 1), _c("div", {staticClass: "ui-blank-box"}, [_c("ui-table", {ref: "table", on: {count: function($event) {
    _vm.count = $event;
  }}, model: {value: _vm.tableConfig, callback: function($$v) {
    _vm.tableConfig = $$v;
  }, expression: "tableConfig"}})], 1)], 1);
};
var staticRenderFns = [];
const GROUP = "pages";
const script = {
  data: () => ({
    count: 0,
    tableConfig: {}
  }),
  created() {
    this.tableConfig = {
      labelPrefix: "@recyclebin.fields.",
      allowOrder: false,
      search: null,
      columns: {
        name: {
          label: "@ui.name",
          as: "text",
          action: (item) => this.actions(item)
        },
        originalId: {
          as: "text",
          width: 300
        },
        createdDate: {
          as: "datetime",
          width: 200
        }
      },
      items: (filter) => {
        filter.group = GROUP;
        return RecycleBinApi.getByQuery(filter);
      }
    };
  },
  methods: {
    actions(item) {
      return Overlay.open({
        component: RecycleBinActionsOverlay,
        display: "editor",
        model: item
      }).then((value) => {
        const deleted = !!value.deleted;
        if (!deleted) {
          hub.$emit("page.update");
          this.$router.push({
            name: "page",
            params: {id: item.originalId}
          });
        } else {
          this.$refs.table.update();
        }
      });
    },
    purge() {
      Overlay.confirmDelete().then((opts) => {
        opts.state("loading");
        RecycleBinApi.deleteByGroup(GROUP).then((res) => {
          opts.state("success");
          opts.hide();
          this.$refs.table.update();
        });
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
component.options.__file = "app/pages/pages/recyclebin/recyclebin.vue";
var recyclebin = component.exports;
export default recyclebin;

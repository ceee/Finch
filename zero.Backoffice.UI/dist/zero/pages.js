import {n as normalizeComponent, P as PageTreeApi} from "./index.js";
import {h as debounce} from "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _vm.area ? _c("div", {staticClass: "ui-linkpicker-area-pages"}, [_c("ui-property", {attrs: {vertical: true}}, [_c("ui-search", {model: {value: _vm.search, callback: function($$v) {
    _vm.search = $$v;
  }, expression: "search"}})], 1), _c("ui-property", {attrs: {vertical: true}}, [_c("ui-tree", _vm._b({ref: "tree", staticClass: "ui-linkpicker-area-pages-tree", attrs: {get: _vm.getTreeItems, selection: _vm.selection}, on: {select: _vm.onSelect}}, "ui-tree", _vm.treeConfig, false))], 1)], 1) : _vm._e();
};
var staticRenderFns = [];
var pages_vue_vue_type_style_index_0_lang = '\n.ui-linkpicker-area-pages-tree\n{\n  margin: 0 -32px 0;\n}\n.ui-linkpicker-area-pages-tree .ui-tree-item.is-selected, \n.ui-linkpicker-area-pages-tree .ui-tree-item:hover:not(.is-disabled)\n{\n  background: var(--color-tree-selected);\n}\n.ui-linkpicker-area-pages-tree\n{\n.ui-tree-item.is-disabled\n  {\n    opacity: .5;\n}\n.ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)\n  {\n    background: var(--color-tree-selected);\n}\n.ui-tree-item.is-selected\n  {\n&:after\n    {\n      font-family: "Feather";\n      content: "\\e83e";\n      font-size: 16px;\n      color: var(--color-primary);\n}\n.ui-tree-item-text\n    {\n      font-weight: bold;\n}\n}\n}\n';
const script = {
  name: "uiLinkpickerAreaPages",
  props: {
    value: {
      type: Object,
      required: true
    },
    area: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    selection: [],
    treeConfig: {
      parent: null,
      active: null,
      mode: "select"
    },
    search: null,
    debouncedSearch: null
  }),
  watch: {
    search() {
      this.debouncedSearch();
    },
    value() {
      this.selection = [this.value.values.id];
    }
  },
  mounted() {
    this.debouncedSearch = debounce(() => this.$refs.tree.refresh(), 300);
    this.selection = this.value.values.id ? [this.value.values.id] : [];
  },
  methods: {
    isValid() {
      return this.selection.length > 0;
    },
    onSelect(id) {
      if (id) {
        this.selection = [id];
        this.value.values = {id};
      } else {
        this.selection = [];
        this.value.values = {id: null};
      }
      this.$emit("change", this.value);
      this.$emit("input", this.value);
    },
    getTreeItems(parent) {
      return PageTreeApi.getChildren(parent, null, this.search).then((res) => {
        res = res.filter((x) => x.id !== "recyclebin");
        res.forEach((item) => {
          item.hasActions = false;
        });
        return res;
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
component.options.__file = "app/components/pickers/linkPicker/areas/pages.vue";
var pages = component.exports;
export default pages;

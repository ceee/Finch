import {n as normalizeComponent, U as UiEditor, O as Overlay, z as UiEditorOverlay, S as Strings, A as Arrays} from "./index.js";
import "./vendor.js";
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "editor-nested", attrs: {depth: _vm.depth}}, [_vm.items.length ? _c("div", {directives: [{name: "sortable", rawName: "v-sortable", value: {onUpdate: _vm.onSortingUpdated}, expression: "{ onUpdate: onSortingUpdated }"}], staticClass: "ui-pick-previews"}, _vm._l(_vm.items, function(item, index) {
    return _c("div", {key: item.id, staticClass: "ui-pick-preview"}, [_c("ui-select-button", {attrs: {icon: _vm.getIcon(item), label: _vm.getName(item), description: _vm.getDescription(item), disabled: _vm.disabled}, on: {click: function($event) {
      return _vm.editItem(item);
    }}}), !_vm.disabled ? _c("ui-icon-button", {attrs: {icon: "fth-x", title: "@ui.close", disabled: _vm.disabled, size: 14}, on: {click: function($event) {
      return _vm.removeItem(index);
    }}}) : _vm._e()], 1);
  }), 0) : _vm._e(), _vm.limit > _vm.items.length ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.addLabel || "@ui.add", disabled: _vm.disabled}, on: {click: _vm.addItem}}) : _vm._e()], 1);
};
var staticRenderFns = [];
const script = {
  name: "UiEditorFieldNested",
  components: {UiEditor},
  props: {
    value: [Array, Object],
    meta: Object,
    depth: Number,
    disabled: Boolean,
    editor: {
      type: Object,
      required: true
    },
    entity: Object,
    limit: {
      type: Number,
      default: 100
    },
    width: {
      type: Number,
      default: 820
    },
    title: String,
    addLabel: String,
    itemLabel: Function,
    itemDescription: Function,
    itemIcon: [String, Function],
    template: {
      type: Object,
      required: true
    }
  },
  watch: {
    value: {
      deep: true,
      handler(val) {
        this.setup(val);
      }
    }
  },
  data: () => ({
    items: [],
    multiple: false
  }),
  mounted() {
    this.setup(this.value);
  },
  methods: {
    setup(value) {
      this.items = JSON.parse(JSON.stringify(value)) || [];
      this.multiple = this.limit > 1;
      if (!this.multiple) {
        this.items = this.items ? [this.items] : [];
      }
    },
    getNewItem() {
      return JSON.parse(JSON.stringify(this.template || {}));
    },
    addItem() {
      if (this.limit <= this.items.length) {
        return;
      }
      this.editItem(this.getNewItem(), true);
      this.onChange();
    },
    editItem(item, isAdd) {
      let parentModel = JSON.parse(JSON.stringify(this.entity));
      if (this.meta && this.meta.parentModel) {
        parentModel.parentModel = this.meta.parentModel;
      }
      return Overlay.open({
        component: UiEditorOverlay,
        display: "editor",
        editor: this.editor,
        title: this.title || "@ui.edit.title",
        model: item,
        width: this.width,
        parentModel,
        create: isAdd
      }).then((value) => {
        if (isAdd) {
          this.items.push(value);
        } else {
          const index = this.items.indexOf(item);
          this.removeItem(index);
          this.items.splice(index, 0, value);
        }
        this.onChange();
      });
    },
    removeItem(index) {
      this.items.splice(index, 1);
      this.onChange();
    },
    onChange() {
      this.$emit("input", this.multiple ? this.items : this.items.length > 0 ? this.items[0] : null);
    },
    getName(item) {
      let name = typeof this.itemLabel === "function" ? this.itemLabel(item) : null;
      return Strings.htmlToText(name != null ? name : "@ui.item");
    },
    getDescription(item) {
      return Strings.htmlToText(typeof this.itemDescription === "function" ? this.itemDescription(item) : "");
    },
    getIcon(item) {
      return typeof this.itemIcon === "function" ? this.itemIcon(item) : this.itemIcon;
    },
    onSortingUpdated(ev) {
      this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
      this.onChange();
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
component.options.__file = "app/editor/fields/nested.vue";
var nested = component.exports;
export default nested;

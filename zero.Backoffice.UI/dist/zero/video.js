import {n as normalizeComponent, Z as LinkpickerOverlay, O as Overlay} from "./index.js";
import {e as extend} from "./vendor.js";
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "ui-linkpicker", class: {"is-disabled": _vm.disabled}}, [_c("input", {ref: "input", attrs: {type: "hidden"}, domProps: {value: _vm.value}}), _vm.canAdd ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: _vm.limit > 1 ? "@ui.add" : "@ui.select", disabled: _vm.disabled}, on: {click: function($event) {
    return _vm.pick();
  }}}) : _vm._e()], 1);
};
var staticRenderFns$1 = [];
var videopicker_vue_vue_type_style_index_0_lang = ".ui-linkpicker-preview {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.ui-linkpicker-preview .ui-icon-button {\n  height: 24px;\n  width: 24px;\n}\n.ui-linkpicker-preview .ui-icon-button i {\n  font-size: 13px;\n}\n.ui-linkpicker-previews + .ui-select-button,\n.ui-linkpicker-preview + .ui-linkpicker-preview {\n  margin-top: 10px;\n}";
const script$1 = {
  name: "uiLinkpicker",
  props: {
    value: {
      type: [Object, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    previews: []
  }),
  watch: {
    value() {
      this.updatePreviews();
    }
  },
  computed: {
    multiple() {
      return this.limit > 1;
    },
    canAdd() {
      return true;
    }
  },
  mounted() {
    this.updatePreviews();
  },
  methods: {
    onChange(value) {
      this.$emit("change", value);
      this.$emit("input", value);
    },
    updatePreviews() {
      this.previews = [];
    },
    remove(id) {
      if (Array.isArray(this.value)) {
        let index = this.value.indexOf(id);
        this.value.splice(index, 1);
        this.onChange(this.value);
      } else {
        this.onChange(this.limit > 1 ? [] : null);
      }
    },
    pick(id) {
      if (this.disabled) {
        return;
      }
      let options = extend({
        title: "Select a link",
        closeLabel: "@ui.close",
        component: LinkpickerOverlay,
        display: "editor",
        model: this.multiple ? id : this.value,
        options: {
          limit: this.limit
        }
      }, typeof this.options === "object" ? this.options : {});
      return Overlay.open(options).then((value) => {
        console.info("confirmed: " + value);
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
component$1.options.__file = "app/components/pickers/videoPicker/videopicker.vue";
var VideoPicker = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("video-picker", _vm._b({attrs: {value: _vm.value, disabled: _vm.disabled, title: "Select a video"}, on: {input: function($event) {
    return _vm.$emit("input", $event);
  }}}, "video-picker", {disabled: _vm.disabled, limit: _vm.limit}, false));
};
var staticRenderFns = [];
const script = {
  props: {
    value: {
      type: [Object, Array],
      default: null
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    config: Object
  },
  components: {VideoPicker}
};
const __cssModules = {};
var component = normalizeComponent(script, render, staticRenderFns, false, injectStyles, null, null, null);
function injectStyles(context) {
  for (let o in __cssModules) {
    this[o] = __cssModules[o];
  }
}
component.options.__file = "app/editor/fields/video.vue";
var video = component.exports;
export default video;

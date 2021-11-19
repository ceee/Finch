var __defProp = Object.defineProperty;
var __hasOwnProp = Object.prototype.hasOwnProperty;
var __getOwnPropSymbols = Object.getOwnPropertySymbols;
var __propIsEnum = Object.prototype.propertyIsEnumerable;
var __defNormalProp = (obj, key, value) => key in obj ? __defProp(obj, key, {enumerable: true, configurable: true, writable: true, value}) : obj[key] = value;
var __assign = (a, b) => {
  for (var prop in b || (b = {}))
    if (__hasOwnProp.call(b, prop))
      __defNormalProp(a, prop, b[prop]);
  if (__getOwnPropSymbols)
    for (var prop of __getOwnPropSymbols(b)) {
      if (__propIsEnum.call(b, prop))
        __defNormalProp(a, prop, b[prop]);
    }
  return a;
};
import {n as normalizeComponent, D as DeliveryProducts, w as ShippingOptionPicker, x as ShippingLines, O as Overlay, y as ShippingOptionsApi, S as Strings$1, z as UiEditorOverlay, B as OrdersApi, b as MediaApi, r as ProductsApi, E as Editor, G as ProductPicker$1, U as UiEditor, A as Arrays} from "./index.js";
import {d as dayjs, a as axios, F as reduce} from "./vendor.js";
var render$g = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-order-delivery", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-module-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), !_vm.create ? _c("ui-button", {attrs: {type: "light onbg", label: "@ui.remove"}, on: {click: function($event) {
        return _vm.remove();
      }}}) : _vm._e(), _c("ui-button", {attrs: {type: "primary", submit: true, label: "@ui.confirm", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("ui-editor", {attrs: {config: "commerce.order-shipping-delivery", disabled: _vm.disabled, meta: _vm.meta}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$g = [];
const script$g = {
  props: {
    config: Object
  },
  components: {DeliveryProducts},
  data: () => ({
    disabled: false,
    loading: true,
    state: "default",
    model: {},
    items: [],
    isPartDelivery: false,
    isPickup: false,
    addTracking: false,
    create: false,
    meta: {}
  }),
  methods: {
    onLoad(form) {
      this.loading = true;
      this.create = this.config.create;
      this.model = JSON.parse(JSON.stringify(this.config.model || {
        id: null,
        number: null,
        trackingNumber: null,
        trackingUrl: null,
        carrierName: null,
        description: null,
        items: [],
        shipmentDate: dayjs().format(),
        isPickup: this.config.isPickup
      }));
      this.meta = {
        products: this.config.products,
        productMeta: this.config.productMeta,
        deliveries: this.config.deliveries
      };
      this.addTracking = this.model.trackingNumber || this.model.trackingUrl ? true : false;
      this.isPartDelivery = this.model.items.length > 0;
      this.loading = false;
    },
    onSubmit(form) {
      this.config.confirm(this.model);
    },
    remove() {
      this.config.confirm({
        remove: true
      });
    }
  }
};
const __cssModules$g = {};
var component$g = normalizeComponent(script$g, render$g, staticRenderFns$g, false, injectStyles$g, null, null, null);
function injectStyles$g(context) {
  for (let o in __cssModules$g) {
    this[o] = __cssModules$g[o];
  }
}
component$g.options.__file = "../zero.Commerce/Plugin/pages/orders/overlays/delivery.vue";
var DeliveryOverlay = component$g.exports;
var render$f = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-deliveries"}, [_c("button", {staticClass: "shop-order-deliveries-item is-add", attrs: {type: "button"}, on: {click: function($event) {
    return _vm.editItem();
  }}}, [_c("ui-icon", {attrs: {symbol: "fth-plus", size: 21}}), _c("span", [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.deliveries.send", expression: "'@shop.order.deliveries.send'"}]})])], 1), _vm._l(_vm.items.slice().reverse(), function(item) {
    return _vm.loaded ? _c("button", {key: item.id, staticClass: "shop-order-deliveries-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.editItem(item);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-package", size: 21}}), _c("span", [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@shop.order.deliveries.package", tokens: {no: item.index}}, expression: "{ key: '@shop.order.deliveries.package', tokens: { no: item.index }}"}]}), _vm._v(" - "), _c("ui-date", {attrs: {value: item.shipmentDate}}), _c("span", {staticClass: "-props -minor"}, [_c("span", {staticClass: "-prop -minor"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@shop.order.deliveries.productsIncluded", tokens: {count: item.productCount, count_all: _vm.allProductsCount}}, expression: "{ key: '@shop.order.deliveries.productsIncluded', tokens: { count: item.productCount, 'count_all': allProductsCount }}"}]})]), _c("span", {staticClass: "-prop -minor"}, [_vm._v("-")]), item.number ? _c("span", {staticClass: "-prop -minor"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.deliveries.packageNumber", expression: "'@shop.order.deliveries.packageNumber'"}]}), _vm._v(": " + _vm._s(item.number))]) : _vm._e()])], 1), _c("ui-icon-button", {attrs: {icon: "fth-chevron-right", size: 17}})], 1) : _vm._e();
  })], 2);
};
var staticRenderFns$f = [];
var deliveries_vue_vue_type_style_index_0_lang = ".shop-order-deliveries {\n  margin-bottom: var(--padding-s);\n}\n.shop-order-deliveries .ui-box:last-child {\n  margin-bottom: var(--padding);\n}\n.shop-order-deliveries-section {\n  padding: 0;\n}\n.shop-order-deliveries-item {\n  display: grid;\n  align-items: center;\n  grid-template-columns: auto 1fr auto;\n  grid-gap: var(--padding-m);\n  width: 100%;\n  line-height: 1.4;\n  padding: var(--padding);\n  background: var(--color-box);\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n  min-height: 100px;\n}\n.shop-order-deliveries-item.is-add {\n  background: transparent;\n  box-shadow: none;\n  border: 1px dashed var(--color-line-dashed-onbg);\n}\n.shop-order-deliveries-item + .shop-order-deliveries-item {\n  margin-top: var(--padding-s);\n}\n.shop-order-deliveries-item .-prop {\n  display: inline-block;\n  margin-right: 8px;\n}\n.shop-order-deliveries-item .-prop.-block {\n  display: block;\n}\n.shop-order-deliveries-item .-props {\n  display: block;\n  margin-top: 0;\n}\n.shop-order-deliveries-item .-prop.-minor {\n  font-size: var(--font-size-xs);\n  color: var(--color-text-dim);\n}";
const script$f = {
  props: {
    model: Object,
    meta: Object
  },
  data: () => ({
    loaded: false,
    disabled: false,
    items: [],
    allProductsCount: 0
  }),
  components: {ShippingOptionPicker, ShippingLines},
  mounted() {
    this.reload();
    this.loaded = true;
  },
  methods: {
    reload() {
      this.allProductsCount = 0;
      this.model.items.forEach((x) => this.allProductsCount += x.quantity);
      let idx = 1;
      this.items = JSON.parse(JSON.stringify(this.model.shipping.deliveries)).map((x) => {
        x.index = idx++;
        x.productCount = this.allProductsCount;
        if (x.items.length) {
          x.productCount = 0;
          x.items.forEach((i) => x.productCount += i.quantity);
        }
        return x;
      });
    },
    getPackageText(item) {
      let count = 0;
      item.items.forEach((x) => count += x.quantity);
      const suffix = count === 0 ? "_all" : count === 1 ? "_1" : "_x";
      return Localization.localize("@shop.order.deliveries.package_text" + suffix, {
        tokens: {
          date: Strings.date(item.shipmentDate),
          count
        }
      });
    },
    editItem(item) {
      return Overlay.open({
        component: DeliveryOverlay,
        display: "editor",
        title: "@shop.order.dispatch.title",
        model: item,
        deliveries: this.items,
        products: this.model.items,
        productMeta: this.meta,
        isPickup: this.model.shipping.isPickup,
        create: !item,
        width: 820
      }).then((value) => {
        if (value.remove) {
          let index = this.items.indexOf(item);
          this.items.splice(index, 1);
        } else if (!item) {
          this.items.push(value);
        } else {
          let index = this.items.indexOf(item);
          this.items.splice(index, 1, value);
        }
        this.model.shipping.deliveries = this.items;
        this.reload();
      });
    },
    openTrackingLink(url) {
      window.open(url, "_blank");
    }
  }
};
const __cssModules$f = {};
var component$f = normalizeComponent(script$f, render$f, staticRenderFns$f, false, injectStyles$f, null, null, null);
function injectStyles$f(context) {
  for (let o in __cssModules$f) {
    this[o] = __cssModules$f[o];
  }
}
component$f.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/deliveries.vue";
var OrderDeliveries = component$f.exports;
var render$e = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-module-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: "@shop.order.shipping.overlay_title", "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", submit: true, label: "@ui.confirm", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("deliveries", {ref: "deliveries", attrs: {model: _vm.config.entity, meta: _vm.meta}}) : _vm._e(), _c("ui-editor", {attrs: {config: "commerce.order-shipping", disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1)];
  }}])});
};
var staticRenderFns$e = [];
const script$e = {
  props: {
    config: Object
  },
  components: {Deliveries: OrderDeliveries},
  data: () => ({
    disabled: false,
    loading: true,
    state: "default",
    meta: {},
    model: {},
    template: {},
    sameAddress: true,
    lastShippingOption: null
  }),
  watch: {
    "model.shippingOptionId"(val) {
      this.onShippingOptionChange(val);
    }
  },
  methods: {
    onLoad(form) {
      this.loading = true;
      this.model = JSON.parse(JSON.stringify(this.config.model));
      this.lastShippingOption = this.model.shippingOptionId;
      this.loading = false;
      if (this.config.isDeliveryAdd) {
        setTimeout(() => {
          this.$refs.deliveries.editItem();
        }, 500);
      }
    },
    onSubmit(form) {
      this.config.confirm(this.model);
    },
    onShippingOptionChange(value) {
      if (this.lastShippingOption == value) {
        return;
      }
      Overlay.confirm({
        title: "@shop.order.shipping.override.title",
        text: "@shop.order.shipping.override.text",
        autoclose: false
      }).then((opts) => {
        opts.state("loading");
        ShippingOptionsApi.getById(value).then((res) => {
          this.model.shippingOptionId = value;
          this.model.name = res.entity.name;
          this.model.price = this.findShippingPrice(res.entity.prices, this.config.sum);
          this.model.isPickup = res.entity.isPickup;
          this.config.entity.shipping.isPickup = this.model.isPickup;
          this.lastShippingOption = value;
          opts.state("success");
          opts.hide();
        });
      }).catch(() => {
        this.model.shippingOptionId = this.lastShippingOption;
      });
    },
    findShippingPrice(prices, sum) {
      let price = 0;
      let done = false;
      prices.forEach((x) => {
        if (!done && sum >= x.from && (sum <= x.to || !x.to)) {
          done = true;
          price = x.price;
        }
      });
      return price;
    }
  }
};
const __cssModules$e = {};
var component$e = normalizeComponent(script$e, render$e, staticRenderFns$e, false, injectStyles$e, null, null, null);
function injectStyles$e(context) {
  for (let o in __cssModules$e) {
    this[o] = __cssModules$e[o];
  }
}
component$e.options.__file = "../zero.Commerce/Plugin/pages/orders/overlays/shipping.vue";
var ShippingOverlay = component$e.exports;
var render$d = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-address"}, [_vm.value.company ? [_vm._v(_vm._s(_vm.value.company)), _c("br")] : _vm._e(), _vm._v(" " + _vm._s(_vm.value.name)), _c("br"), _vm.value.address ? [_vm._v(_vm._s(_vm.value.address) + " " + _vm._s(_vm.value.addressNo))] : _vm._e(), _vm.value.addressLine1 ? [_vm._v("/ " + _vm._s(_vm.value.addressLine1))] : _vm._e(), _vm.value.addressLine2 ? [_vm._v("/ " + _vm._s(_vm.value.addressLine2))] : _vm._e(), _vm.value.address ? _c("br") : _vm._e(), _vm.value.zip || _vm.value.city ? [_vm._v(_vm._s(_vm.value.zip) + " " + _vm._s(_vm.value.city)), _c("br")] : _vm._e(), _vm.value.country ? [_vm._v(_vm._s(_vm.value.country))] : _vm._e()], 2);
};
var staticRenderFns$d = [];
var address_vue_vue_type_style_index_0_lang = ".shop-order-address {\n  line-height: 1.5;\n}";
const script$d = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    }
  }
};
const __cssModules$d = {};
var component$d = normalizeComponent(script$d, render$d, staticRenderFns$d, false, injectStyles$d, null, null, null);
function injectStyles$d(context) {
  for (let o in __cssModules$d) {
    this[o] = __cssModules$d[o];
  }
}
component$d.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/address.vue";
var OrderAddress = component$d.exports;
var render$c = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-addresses"}, [_c("ui-property", {attrs: {label: "@shop.order.fields.contact", "hide-label": false, vertical: true}, scopedSlots: _vm._u([{key: "label-after", fn: function() {
    return [!_vm.disabled ? _c("button", {staticClass: "shop-order-property-edit", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.editCustomer(_vm.value.customer);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-edit-2", size: 14}})], 1) : _vm._e()];
  }, proxy: true}])}, [_vm.value.customer.id ? _c("router-link", {directives: [{name: "localize", rawName: "v-localize:title", value: "@shop.order.fields.customer", expression: "'@shop.order.fields.customer'", arg: "title"}], staticClass: "shop-order-address-link", attrs: {to: {name: "commerce-customers-edit", params: {id: _vm.value.customer.id}}}}, [_c("span", {staticClass: "-icon"}, [_c("ui-icon", {attrs: {symbol: "fth-user", size: 14}})], 1), _c("span", [_vm._v(_vm._s(_vm.value.customer.name))]), _vm._v(" "), _c("span", {staticClass: "-minor"}, [_vm._v("#" + _vm._s(_vm.value.customer.number))])]) : _c("span", {directives: [{name: "localize", rawName: "v-localize:title", value: "@shop.order.fields.customer", expression: "'@shop.order.fields.customer'", arg: "title"}], staticClass: "shop-order-address-link"}, [_c("span", {staticClass: "-icon"}, [_c("ui-icon", {attrs: {symbol: "fth-user", size: 14}})], 1), _c("span", [_vm._v(_vm._s(_vm.value.customer.name))])]), _c("a", {directives: [{name: "localize", rawName: "v-localize:title", value: "@shop.order.fields.email", expression: "'@shop.order.fields.email'", arg: "title"}], staticClass: "shop-order-address-link", attrs: {href: "mailto:" + _vm.value.customer.email}}, [_c("span", {staticClass: "-icon"}, [_c("ui-icon", {attrs: {symbol: "fth-mail", size: 14}})], 1), _c("span", [_vm._v(_vm._s(_vm.value.customer.email))])]), _vm.value.customer.phoneNumber ? _c("a", {directives: [{name: "localize", rawName: "v-localize:title", value: "@shop.order.fields.phone", expression: "'@shop.order.fields.phone'", arg: "title"}], staticClass: "shop-order-address-link", attrs: {href: "tel:" + _vm.value.customer.phoneNumber}}, [_c("span", {staticClass: "-icon"}, [_c("ui-icon", {attrs: {symbol: "fth-smartphone", size: 14}})], 1), _c("span", [_vm._v(_vm._s(_vm.value.customer.phoneNumber))])]) : _vm._e()], 1), _c("hr"), _c("ui-property", {attrs: {label: _vm.value.shipping ? "@shop.order.fields.invoiceAddress" : "@shop.order.fields.address", vertical: true, "is-text": true}, scopedSlots: _vm._u([{key: "label-after", fn: function() {
    return [!_vm.disabled ? _c("button", {staticClass: "shop-order-property-edit", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.editAddress(_vm.value.address);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-edit-2", size: 14}})], 1) : _vm._e()];
  }, proxy: true}])}, [_c("order-address", {model: {value: _vm.value.address, callback: function($$v) {
    _vm.$set(_vm.value, "address", $$v);
  }, expression: "value.address"}})], 1), _vm.value.shipping ? [_c("hr"), _c("ui-property", {attrs: {label: "@shop.order.fields.shippingAddress", vertical: true, "is-text": true}, scopedSlots: _vm._u([{key: "label-after", fn: function() {
    return [!_vm.disabled ? _c("button", {staticClass: "shop-order-property-edit", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.editShipping(_vm.value.shipping);
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-edit-2", size: 14}})], 1) : _vm._e()];
  }, proxy: true}], null, false, 3498176099)}, [_vm.value.shipping.name ? _c("span", [_vm._v(_vm._s(_vm.value.shipping.name))]) : _vm._e(), !_vm.value.shipping.address ? _c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.addresses.sameShippingAddress", expression: "'@shop.order.addresses.sameShippingAddress'"}]}) : _c("order-address", {model: {value: _vm.value.shipping.address, callback: function($$v) {
    _vm.$set(_vm.value.shipping, "address", $$v);
  }, expression: "value.shipping.address"}})], 1)] : _vm._e()], 2);
};
var staticRenderFns$c = [];
var addresses_vue_vue_type_style_index_0_lang = ".shop-order-addresses {\n  width: 100%;\n}\n.shop-order-addresses .shop-order-property-edit {\n  float: right;\n}\n.shop-order-addresses .ui-property-content {\n  line-height: 1.5;\n}\n.shop-order-address .ui-property + .ui-property {\n  margin-top: 20px;\n}\n.shop-order-address .ui-property-content {\n  line-height: 1.5;\n}\n.shop-order-address .ui-property-label {\n  width: 200px;\n  padding-right: var(--padding);\n}\n.shop-order-property-edit {\n  font-size: var(--font-size);\n  padding: 5px;\n  margin-left: 3px;\n  margin-top: -3px;\n  margin-bottom: -5px;\n  color: var(--color-text-dim);\n}\n.shop-order-property-edit:hover {\n  color: var(--color-text);\n}\na.shop-order-address-link, .shop-order-address-link {\n  display: flex;\n  align-items: center;\n  color: var(--color-text);\n  margin-top: 0;\n  text-decoration: none;\n  /*span\n  {\n    text-decoration: underline dotted var(--color-text) !important;\n    text-underline-offset: 3px;\n  }*/\n}\na.shop-order-address-link + .shop-order-address-link, .shop-order-address-link + .shop-order-address-link {\n  margin-top: 15px;\n}\na.shop-order-address-link .-minor, .shop-order-address-link .-minor {\n  margin-left: 0.5em;\n  font-size: var(--font-size-xs) !important;\n  color: var(--color-text-dim) !important;\n}\na.shop-order-address-link .-icon, .shop-order-address-link .-icon {\n  width: 32px;\n  height: 32px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  border-radius: 100%;\n  margin-right: 12px;\n  background: var(--color-button-light);\n  color: var(--color-text);\n  text-align: center;\n  font-size: 15px;\n}";
const script$c = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  components: {OrderAddress},
  methods: {
    getCurrency(value) {
      return Strings$1.currency(value);
    },
    editAddress(data) {
      return Overlay.open({
        component: UiEditorOverlay,
        display: "editor",
        editor: "commerce.order-address",
        title: "@ui.edit.title",
        model: data,
        width: 800,
        create: false
      }).then((value) => {
        this.$emit("address", value);
      });
    },
    editShipping(data, addressChange) {
      return Overlay.open({
        component: ShippingOverlay,
        display: "editor",
        model: data,
        entity: this.value,
        sum: this.value.payablePrice - (this.value.shipping ? this.value.shipping.price : 0),
        width: 720,
        addressChange
      }).then((value) => {
        this.$emit("shipping", value);
      });
    },
    editCustomer(data) {
      return Overlay.open({
        component: UiEditorOverlay,
        display: "editor",
        editor: "commerce.order-contact",
        title: "@shop.order.contact.headline",
        model: data,
        width: 800,
        create: false
      }).then((value) => {
        this.$emit("customer", value);
      });
    }
  }
};
const __cssModules$c = {};
var component$c = normalizeComponent(script$c, render$c, staticRenderFns$c, false, injectStyles$c, null, null, null);
function injectStyles$c(context) {
  for (let o in __cssModules$c) {
    this[o] = __cssModules$c[o];
  }
}
component$c.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/addresses.vue";
var OrderAddresses = component$c.exports;
var render$b = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-overlay-editor", {staticClass: "shop-order-revisions", scopedSlots: _vm._u([{key: "header", fn: function() {
    return [_c("ui-header-bar", {attrs: {title: "@shop.order.states.history_overlay", "back-button": false, "close-button": true}})];
  }, proxy: true}, {key: "footer", fn: function() {
    return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}})];
  }, proxy: true}])}, [_c("div", {staticClass: "ui-box shop-order-revisions-inner"}, [_c("ui-revisions", {attrs: {get: _vm.getRevisions}})], 1)]);
};
var staticRenderFns$b = [];
var revisions_vue_vue_type_style_index_0_lang = ".shop-order-revisions > content {\n  position: relative;\n  padding-top: 0 !important;\n}\n.shop-order-revisions .ui-box {\n  margin: 0;\n}";
const script$b = {
  props: {
    config: Object
  },
  methods: {
    getRevisions(page) {
      return OrdersApi.getRevisions(this.config.id, page);
    }
  }
};
const __cssModules$b = {};
var component$b = normalizeComponent(script$b, render$b, staticRenderFns$b, false, injectStyles$b, null, null, null);
function injectStyles$b(context) {
  for (let o in __cssModules$b) {
    this[o] = __cssModules$b[o];
  }
}
component$b.options.__file = "../zero.Commerce/Plugin/pages/orders/overlays/revisions.vue";
var OrderRevisionsOverlay = component$b.exports;
const base = "commerceDocuments/";
var DocumentsApi = {
  getAllForOrder(id) {
    return axios.get(base + "getAllForOrder", {params: {orderId: id}}).then((res) => Promise.resolve(res.data));
  }
};
var render$a = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-order-add-document", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("h2", {directives: [{name: "localize", rawName: "v-localize", value: "Add document", expression: "'Add document'"}], staticClass: "ui-headline"}), _c("div", {staticClass: "shop-order-add-document-items"}, [_c("button", {staticClass: "shop-order-add-document-item", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.select(null);
    }}}, [_c("ui-icon", {staticClass: "shop-order-add-document-item-icon", attrs: {symbol: "fth-upload", size: 22}}), _c("span", {staticClass: "shop-order-add-document-item-text"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "Upload", expression: "'Upload'"}]}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: "Add a custom file without using a template", expression: "'Add a custom file without using a template'"}], staticClass: "-minor"})])], 1)]), _c("hr"), _c("div", {staticClass: "shop-order-add-document-items"}, _vm._l(_vm.items, function(item) {
      return _c("button", {staticClass: "shop-order-add-document-item", attrs: {type: "button"}, on: {click: function($event) {
        return _vm.select(item);
      }}}, [_c("ui-icon", {staticClass: "shop-order-add-document-item-icon", attrs: {symbol: item.isUpload ? "fth-upload" : "fth-file-text", size: 22}}), _c("span", {staticClass: "shop-order-add-document-item-text"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.name, expression: "item.name"}]}), _c("span", {directives: [{name: "localize", rawName: "v-localize", value: item.description, expression: "item.description"}], staticClass: "-minor"})])], 1);
    }), 0)];
  }}])});
};
var staticRenderFns$a = [];
var addDocument_vue_vue_type_style_index_0_lang = ".shop-order-add-document {\n  text-align: left;\n}\n.shop-order-add-document-items {\n  margin: 24px -16px 0;\n}\n.shop-order-add-document-item {\n  display: grid;\n  width: 100%;\n  transition: background 0.2s, transform 0.2s, opacity 0.2s;\n  grid-template-columns: 48px 1fr auto;\n  gap: 6px;\n  height: 100%;\n  align-items: center;\n  position: relative;\n  color: var(--color-text);\n  padding: 16px;\n  border-radius: var(--radius);\n}\n.shop-order-add-document-item:hover, .shop-order-add-document-item:focus {\n  background: var(--color-box-nested);\n}\n.shop-order-add-document-item:hover .shop-order-add-document-item-icon, .shop-order-add-document-item:focus .shop-order-add-document-item-icon {\n  color: var(--color-text);\n}\n.shop-order-add-document-item + .shop-order-add-document-item {\n  margin-top: 0;\n}\n.shop-order-add-document-item-text {\n  display: flex;\n  flex-direction: column;\n  line-height: 1.3;\n}\n.shop-order-add-document-item-text .-minor {\n  color: var(--color-text-dim);\n  margin-top: 3px;\n}\n.shop-order-add-document-item-icon {\n  font-size: 22px;\n  line-height: 1;\n  font-weight: 400;\n  position: relative;\n  top: -2px;\n  left: 4px;\n  color: var(--color-text);\n  transition: color 0.2s ease;\n}";
const script$a = {
  props: {
    model: Object,
    config: Object
  },
  data: () => ({
    loading: true,
    items: [],
    selected: null,
    disabled: false
  }),
  methods: {
    onLoad(form) {
      form.load(DocumentsApi.getAllForOrder(this.config.id)).then((response) => {
        this.items = response;
        this.loading = false;
      });
    },
    onSubmit(form) {
    },
    select(item) {
    }
  }
};
const __cssModules$a = {};
var component$a = normalizeComponent(script$a, render$a, staticRenderFns$a, false, injectStyles$a, null, null, null);
function injectStyles$a(context) {
  for (let o in __cssModules$a) {
    this[o] = __cssModules$a[o];
  }
}
component$a.options.__file = "../zero.Commerce/Plugin/pages/orders/overlays/add-document.vue";
var AddDocumentOverlay = component$a.exports;
var render$9 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "ui-module-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: "@shop.order.documents.title", "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", submit: true, label: "@ui.confirm", state: form.state, disabled: _vm.disabled}})];
    }, proxy: true}], null, true)}, [_c("div", {staticClass: "shop-order-documents"}, [_c("button", {staticClass: "shop-order-documents-item is-add", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.addItem();
    }}}, [_c("ui-icon", {attrs: {symbol: "fth-plus", size: 21}}), _c("span", [_c("strong", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.documents.add_new", expression: "'@shop.order.documents.add_new'"}]})])], 1), _vm._l(_vm.model.slice().reverse(), function(item) {
      return _c("a", {key: item.id, staticClass: "shop-order-documents-item", attrs: {href: item.url, target: "_blank"}}, [_c("ui-icon", {attrs: {symbol: "fth-file-text", size: 21}}), _c("span", [_c("strong", [_vm._v(_vm._s(item.name))]), item.number ? _c("span", [_vm._v(" #" + _vm._s(item.number))]) : _vm._e(), _c("span", {staticClass: "-props -minor"}, [_c("span", {directives: [{name: "filesize", rawName: "v-filesize", value: item.size, expression: "item.size"}], staticClass: "-prop -minor"}), _c("span", {staticClass: "-prop -minor"}, [_vm._v("-")]), _c("span", {staticClass: "-prop -minor"}, [_c("ui-date", {attrs: {value: item.createdDate}})], 1)])]), !_vm.disabled ? _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
        return [_c("ui-icon-button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.actions", expression: "'@ui.actions'", arg: "title"}], attrs: {icon: "fth-more-horizontal", size: 15}})];
      }, proxy: true}], null, true)}, [_c("ui-dropdown-button", {attrs: {label: "@ui.edit.title", icon: "fth-edit-2"}, on: {click: function($event) {
        return _vm.editItem(item);
      }}}), _c("ui-dropdown-button", {attrs: {label: "@ui.delete", negative: true, icon: "fth-trash", confirm: true}, on: {click: function($event) {
        return _vm.$emit("remove", item);
      }}})], 1) : _vm._e()], 1);
    })], 2)])];
  }}])});
};
var staticRenderFns$9 = [];
var documents_vue_vue_type_style_index_0_lang = ".shop-order-documents {\n  margin-bottom: var(--padding-s);\n}\n.shop-order-documents .ui-box:last-child {\n  margin-bottom: var(--padding);\n}\n.shop-order-documents-section {\n  padding: 0;\n}\n.shop-order-documents-item {\n  display: grid;\n  align-items: center;\n  grid-template-columns: auto 1fr auto;\n  grid-gap: var(--padding-m);\n  width: 100%;\n  line-height: 1.4;\n  padding: var(--padding);\n  background: var(--color-box);\n  border-radius: var(--radius);\n  box-shadow: var(--shadow-short);\n  min-height: 100px;\n  color: var(--color-text);\n}\n.shop-order-documents-item.is-add {\n  background: transparent;\n  box-shadow: none;\n  border: 1px dashed var(--color-line-dashed-onbg);\n}\n.shop-order-documents-item + .shop-order-documents-item {\n  margin-top: var(--padding-s);\n}\n.shop-order-documents-item .-prop {\n  display: inline-block;\n  margin-right: 8px;\n}\n.shop-order-documents-item .-prop.-block {\n  display: block;\n}\n.shop-order-documents-item .-props {\n  display: block;\n  margin-top: 0;\n}\n.shop-order-documents-item .-prop.-minor {\n  font-size: var(--font-size-xs);\n  color: var(--color-text-dim);\n}";
const script$9 = {
  props: {
    config: Object
  },
  data: () => ({
    disabled: false,
    state: "default",
    meta: {},
    model: [],
    template: {}
  }),
  methods: {
    onLoad(form) {
      this.model = JSON.parse(JSON.stringify(this.config.model));
      this.model.forEach((item) => {
        item.url = OrdersApi.getDocumentUrl(this.config.entity.id, item.id);
      });
    },
    onSubmit(form) {
      this.config.confirm(this.model);
    },
    addItem() {
      return Overlay.open({
        component: AddDocumentOverlay,
        id: this.config.entity.id
      }).then((item) => {
        console.info(item);
      });
    }
  }
};
const __cssModules$9 = {};
var component$9 = normalizeComponent(script$9, render$9, staticRenderFns$9, false, injectStyles$9, null, null, null);
function injectStyles$9(context) {
  for (let o in __cssModules$9) {
    this[o] = __cssModules$9[o];
  }
}
component$9.options.__file = "../zero.Commerce/Plugin/pages/orders/overlays/documents.vue";
var DocumentsOverlay = component$9.exports;
var render$8 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-documents-preview"}, [!_vm.value.documents.length ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: "@shop.order.documents.add"}, on: {click: function($event) {
    return _vm.editDocuments(true);
  }}}) : _c("ui-select-button", {attrs: {icon: "fth-file-text", label: "@shop.order.documents.documentsCount", tokens: {count: _vm.value.documents.length}}, on: {click: function($event) {
    return _vm.editDocuments(false);
  }}})], 1);
};
var staticRenderFns$8 = [];
const script$8 = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({}),
  created() {
  },
  methods: {
    editDocuments(isAdd) {
      return Overlay.open({
        component: DocumentsOverlay,
        display: "editor",
        model: this.value.documents,
        entity: this.value,
        isAdd,
        width: 620
      }).then((value) => {
        this.$emit("documents", value);
      });
    }
  }
};
const __cssModules$8 = {};
var component$8 = normalizeComponent(script$8, render$8, staticRenderFns$8, false, injectStyles$8, null, null, null);
function injectStyles$8(context) {
  for (let o in __cssModules$8) {
    this[o] = __cssModules$8[o];
  }
}
component$8.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/documents.vue";
var OrderDocuments = component$8.exports;
var render$7 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-states"}, [_c("ui-property", {attrs: {label: "@shop.order.states.orderState", vertical: true}}, [_vm.preview ? _c("div", {staticClass: "shop-order-state"}, [_c("ui-pick", {ref: "picker", attrs: {config: _vm.statePickerConfig, disabled: _vm.disabled}, on: {select: _vm.onChange}, model: {value: _vm.value.state.stateId, callback: function($$v) {
    _vm.$set(_vm.value.state, "stateId", $$v);
  }, expression: "value.state.stateId"}}), _c("ui-select-button", {attrs: {icon: _vm.preview.icon, label: _vm.preview.name, description: _vm.preview.text}, on: {click: function($event) {
    return _vm.$refs.picker.pick();
  }}}, [_c("ui-icon", {staticClass: "shop-order-state-arrow-down", attrs: {symbol: "fth-chevron-down"}})], 1)], 1) : _vm._e()]), _vm.paymentPreview ? _c("ui-property", {attrs: {label: "@shop.order.states.paymentState", vertical: true}}, [_c("div", {staticClass: "shop-order-state"}, [_c("ui-select-button", {attrs: {icon: _vm.paymentPreview.icon, label: _vm.paymentPreview.name, description: _vm.paymentPreview.text}}, [_c("ui-icon", {staticClass: "shop-order-state-arrow-down", attrs: {symbol: "fth-chevron-down"}})], 1)], 1)]) : _vm._e(), _c("ui-property", {attrs: {label: "@shop.order.tabs.documents", vertical: true}}, [_c("order-documents", {attrs: {disabled: _vm.disabled}, model: {value: _vm.value, callback: function($$v) {
    _vm.value = $$v;
  }, expression: "value"}})], 1), _vm.value.shipping ? _c("ui-property", {attrs: {label: "@shop.order.deliveries.title", vertical: true}}, [!_vm.value.shipping.deliveries.length ? _c("ui-select-button", {attrs: {icon: "fth-plus", label: "@shop.order.deliveries.add"}, on: {click: function($event) {
    return _vm.editDeliveries(true);
  }}}) : _c("ui-select-button", {attrs: {icon: "fth-package", label: "@shop.order.deliveries.packagesSent", tokens: {count: _vm.value.shipping.deliveries.length}}, on: {click: function($event) {
    return _vm.editDeliveries(false);
  }}})], 1) : _vm._e()], 1);
};
var staticRenderFns$7 = [];
var states_vue_vue_type_style_index_0_lang = ".shop-order-states {\n  display: grid;\n  grid-template-columns: repeat(4, 1fr);\n  grid-gap: 20px;\n  justify-content: space-between;\n  align-items: flex-start;\n}\n.shop-order-states .ui-property {\n  margin: 0;\n  margin-top: 0 !important;\n  padding: 0;\n  border-top: none !important;\n}\n.shop-order-states .ui-property-label {\n  margin-bottom: 0;\n}\n.shop-order-states .ui-select-button b {\n  font-weight: bold !important;\n}\n.shop-order-states .ui-pick-overlay-item {\n  padding: 14px 18px;\n}\n.shop-order-states .ui-pick-overlay-items {\n  max-height: 360px;\n}\n.shop-order-states .ui-pick-overlay-item.is-selected {\n  background: var(--color-tree-selected);\n}\n.shop-order-state {\n  display: block;\n  width: 100%;\n  position: relative;\n}\n.shop-order-state .ui-pick {\n  position: relative;\n  top: 40px;\n}\n.shop-order-state .ui-dropdown {\n  width: 420px !important;\n  max-width: 420px !important;\n}\n.shop-order-state .ui-select-button {\n  width: 100%;\n}\n.shop-order-state-arrow-down {\n  margin-left: 12px;\n  justify-self: flex-end;\n}\n.shop-order-state + .shop-order-state {\n  margin-left: var(--padding-l);\n}";
const icons = {
  open: "fth-loader",
  processing: "fth-rotate-cw",
  cancelled: "fth-x",
  completed: "fth-check",
  none: "fth-loader",
  pending: "fth-rotate-cw",
  captured: "fth-check",
  authorized: "fth-check",
  error: "fth-alert-circle",
  refunded: "fth-chevrons-left"
};
const script$7 = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    orderStates: {
      type: Array,
      default: () => []
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    },
    meta: {
      type: Object
    }
  },
  components: {OrderDocuments},
  data: () => ({
    statePickerConfig: {},
    preview: null,
    paymentPreview: null
  }),
  created() {
    this.orderStates.map((x) => {
      x.icon = icons[x.underlyingState];
      return x;
    });
    this.rebuild(this.value.state.stateId);
    this.statePickerConfig = {
      scope: "order-state",
      title: "Order state",
      search: {
        enabled: false
      },
      items: this.orderStates,
      limit: 1,
      multiple: false,
      keys: {
        description: "description"
      },
      list: {
        description: true
      },
      preview: {
        enabled: false
      }
    };
  },
  methods: {
    rebuild(stateId) {
      let orderState = this.orderStates.find((x) => x.id === stateId);
      let stateHistoryItem = this.value.stateHistory[this.value.stateHistory.length - 1];
      this.preview = __assign(__assign({}, orderState), {
        text: Strings$1.date(stateHistoryItem.createdDate, "long")
      });
      if (this.value.payment) {
        this.paymentPreview = {
          name: "@shop.payment.states." + this.value.payment.status,
          icon: icons[this.value.payment.status],
          text: this.value.payment.method.name
        };
      }
    },
    onChange(id, item) {
      this.rebuild(id);
    },
    openHistory() {
      return Overlay.open({
        component: OrderRevisionsOverlay,
        id: this.value.id,
        display: "editor",
        width: 820
      });
    },
    editDeliveries(isAdd) {
      return Overlay.open({
        component: ShippingOverlay,
        display: "editor",
        model: this.value.shipping,
        entity: this.value,
        sum: this.value.price - (this.value.shipping ? this.value.shipping.price : 0),
        isDeliveryAdd: isAdd,
        width: 720
      }).then((value) => {
        this.$emit("shipping", value);
      });
    }
  }
};
const __cssModules$7 = {};
var component$7 = normalizeComponent(script$7, render$7, staticRenderFns$7, false, injectStyles$7, null, null, null);
function injectStyles$7(context) {
  for (let o in __cssModules$7) {
    this[o] = __cssModules$7[o];
  }
}
component$7.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/states.vue";
var OrderStates = component$7.exports;
var render$6 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {staticClass: "shop-product-preview-overlay", class: {"has-image": !!_vm.image}, on: {load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [!_vm.loading && _vm.model.image && _vm.image ? _c("div", {staticClass: "shop-product-preview-overlay-image"}, [_c("img", {attrs: {src: _vm.image}})]) : _vm._e(), _c("div", {staticClass: "shop-product-preview-overlay-data"}, [_c("h2", {staticClass: "ui-headline"}, [_vm._v(_vm._s(_vm.model.name))]), !_vm.loading ? _c("hr") : _vm._e(), _vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "is-narrow"}, [_vm.model.channel ? _c("ui-property", {attrs: {label: "@shop.product.fields.channelId", vertical: false, "is-text": true}}, [_c("button", {staticClass: "ui-link", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.goTo("commerce-channels-edit", _vm.model.channel.id);
    }}}, [_vm._v(_vm._s(_vm.model.channel.name))])]) : _vm._e(), _vm.model.manufacturer ? _c("ui-property", {attrs: {label: "@shop.product.fields.manufacturerId", vertical: false, "is-text": true}}, [_c("button", {staticClass: "ui-link", attrs: {type: "button"}, on: {click: function($event) {
      return _vm.goTo("commerce-manufacturers-edit", _vm.model.manufacturer.id);
    }}}, [_vm._v(_vm._s(_vm.model.manufacturer.name))])]) : _vm._e(), _vm.model.categories.length > 0 ? _c("ui-property", {attrs: {label: "@shop.product.fields.categoryIds", vertical: false, "is-text": true}}, _vm._l(_vm.model.categories, function(category) {
      return _c("button", {staticClass: "ui-link", attrs: {type: "button"}, on: {click: function($event) {
        return _vm.goTo("commerce-catalogue-category", category.id);
      }}}, [_vm._v(_vm._s(category.name))]);
    }), 0) : _vm._e(), !_vm.hasVariant ? _c("ui-property", {attrs: {label: "@shop.product.fields.description", "is-text": true, vertical: true}}, [_c("div", {staticClass: "shop-product-preview-overlay-rte", domProps: {innerHTML: _vm._s(_vm.model.description)}})]) : _vm._e(), _vm._l(_vm.model.properties, function(property) {
      return _c("ui-property", {key: property.id, attrs: {label: property.id, vertical: false, "is-text": true}}, [_vm._v(_vm._s(property.name))]);
    }), _vm.hasVariant ? _c("ui-property", {attrs: {label: "@shop.product.preview.price", "is-text": true, vertical: false}}, [_c("span", {directives: [{name: "currency", rawName: "v-currency", value: _vm.model.price, expression: "model.price"}]})]) : _vm._e(), _vm.hasVariant && _vm.model.sku ? _c("ui-property", {attrs: {label: "@shop.product.fields.variant.sku", "is-text": true, vertical: false}}, [_vm._v(_vm._s(_vm.model.sku))]) : _vm._e(), _vm.hasVariant && _vm.model.ean ? _c("ui-property", {attrs: {label: "@shop.product.fields.variant.ean", "is-text": true, vertical: false}}, [_vm._v(_vm._s(_vm.model.ean))]) : _vm._e(), _vm.model.stock.length > 0 ? _c("ui-property", {staticClass: "shop-product-preview-overlay-stock", attrs: {label: "@shop.product.fields.variant.stock", "is-text": true, vertical: false}}, _vm._l(_vm.model.stock, function(stock) {
      return _c("div", [_c("b", [_c("i", {staticClass: "fth-package"}), _vm._v(" " + _vm._s(stock.count))]), _vm._v(" (" + _vm._s(stock.provider) + ")")]);
    }), 0) : _vm._e(), _c("hr"), _c("router-link", {staticClass: "ui-button type-light type-onbg shop-product-preview-overlay-go", attrs: {to: {name: "commerce-products-edit", params: {id: _vm.config.id}}}, on: {click: function($event) {
      $event.preventDefault();
      $event.stopPropagation();
      return _vm.goToDetail($event);
    }}}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.product.preview.goToDetail", expression: "'@shop.product.preview.goToDetail'"}]})]), _c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.close}})], 2) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$6 = [];
var productPreview_vue_vue_type_style_index_0_lang = ".shop-product-preview-overlay {\n  text-align: left;\n}\n.shop-product-preview-overlay .ui-property + .ui-property {\n  margin-top: 20px;\n}\n.shop-product-preview-overlay .ui-property-label {\n  padding-right: var(--padding);\n}\n.shop-product-preview-overlay .ui-loading {\n  position: absolute;\n  left: 50%;\n  top: 50%;\n  margin: -14px 0 0 -14px;\n}\n.shop-product-preview-overlay hr {\n  margin: var(--padding) 0;\n  border-color: var(--color-bg-shade-4);\n}\n.shop-product-preview-overlay .ui-link + .ui-link {\n  margin-left: 1em;\n}\n.shop-product-preview-overlay .ui-button-icon {\n  margin-left: 12px;\n}\n.shop-product-preview-overlay-rte {\n  margin-top: 5px;\n  line-height: 1.5;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  -webkit-box-orient: vertical;\n  -webkit-line-clamp: 2;\n  display: -webkit-box;\n  max-height: 42px;\n}\n.shop-product-preview-overlay-rte p {\n  margin: 0;\n}\n.shop-product-preview-overlay {\n  display: block;\n}\n.shop-product-preview-overlay.has-image {\n  display: grid;\n  grid-template-columns: 5fr 3fr;\n}\n.shop-product-preview-overlay-image {\n  display: flex;\n  justify-content: center;\n  align-items: center;\n  margin-left: -32px;\n  margin-right: 32px;\n}\n.shop-product-preview-overlay-image img {\n  max-width: 75%;\n  max-height: 80%;\n}\n.shop-product-preview-overlay-data {\n  margin: -32px;\n  margin-top: -40px;\n  padding: var(--padding);\n  background: var(--color-box-nested);\n}\n.has-image .shop-product-preview-overlay-data {\n  border-left: 1px solid var(--color-bg-shade-4);\n}\n.shop-product-preview-overlay-stock .ui-property-content {\n  line-height: 1.5;\n}\n.shop-product-preview-overlay-stock .fth-package {\n  margin-right: 3px;\n}";
const script$6 = {
  props: {
    config: Object
  },
  data: () => ({
    loading: true,
    state: "default",
    model: {
      categories: [],
      properties: []
    }
  }),
  computed: {
    hasVariant() {
      return this.model.properties.length > 0;
    },
    image() {
      let source = MediaApi.getImageSource(this.model.image, false);
      return source ? source.replace("productListing", "product") : null;
    }
  },
  methods: {
    onLoad(form) {
      this.loading = true;
      form.load(ProductsApi.getPreview(this.config.id, this.config.variantId)).then((response) => {
        this.model = response;
        this.loading = false;
      });
    },
    goTo(name, id) {
      this.config.close();
      this.$router.push({name, params: {id}});
    },
    goToDetail(e) {
      e.preventDefault();
      this.config.close();
      this.$router.push({name: "commerce-products-edit", params: {id: this.config.id}});
    }
  }
};
const __cssModules$6 = {};
var component$6 = normalizeComponent(script$6, render$6, staticRenderFns$6, false, injectStyles$6, null, null, null);
function injectStyles$6(context) {
  for (let o in __cssModules$6) {
    this[o] = __cssModules$6[o];
  }
}
component$6.options.__file = "../zero.Commerce/Plugin/pages/catalogue-old/overlays/product-preview.vue";
var ProductPreviewOverlay = component$6.exports;
var render$5 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-item-pricing"}, [_c("div", {staticClass: "-input -price"}, [_c("ui-property", {attrs: {label: "Price"}}, [_c("ui-currency", {model: {value: _vm.price, callback: function($$v) {
    _vm.price = $$v;
  }, expression: "price"}})], 1)], 1), _vm.discountable ? _c("div", {staticClass: "-op"}, [_vm._v("\u2212")]) : _vm._e(), _vm.discountable ? _c("div", {staticClass: "-input -discount"}, [_c("ui-property", {attrs: {label: "Discount"}}, [_c("ui-currency", {model: {value: _vm.discount, callback: function($$v) {
    _vm.discount = $$v;
  }, expression: "discount"}})], 1)], 1) : _vm._e(), _c("div", {staticClass: "-op"}, [_vm._v("\xD7")]), _c("div", {staticClass: "-input -quantity"}, [_c("ui-property", {attrs: {label: "Quantity"}}, [_c("input", {staticClass: "ui-input", attrs: {type: "text", disabled: _vm.disabled}, domProps: {value: _vm.quantity}, on: {input: function($event) {
    return _vm.onQuantityChange($event.target.value);
  }}})])], 1), _c("div", {staticClass: "-op"}, [_vm._v("=")]), _c("div", {staticClass: "-input -sum"}, [_c("ui-property", [_c("span", {staticClass: "-sumoutput", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.sum))}})])], 1)]);
};
var staticRenderFns$5 = [];
var orderItemPricing_vue_vue_type_style_index_0_lang = ".shop-order-item-pricing {\n  display: flex;\n  justify-content: stretch;\n}\n.shop-order-item-pricing .-input {\n  flex-grow: 1;\n}\n.shop-order-item-pricing .-op {\n  flex: 0 0 32px;\n  text-align: center;\n  align-self: center;\n  position: relative;\n  top: 16px;\n  font-weight: 700;\n}\n.shop-order-item-pricing .-sum {\n  flex: 0 0 100px;\n  text-align: right;\n}\n.shop-order-item-pricing .-quantity {\n  flex: 0 0 120px;\n}\n.shop-order-item-pricing .-sumoutput {\n  position: relative;\n  top: 46px;\n  font-size: var(--font-size-l);\n  font-weight: 700;\n}";
const script$5 = {
  name: "OrderItemPricing",
  props: {
    value: {
      type: Number
    },
    entity: {
      type: Object
    },
    discountable: {
      type: Boolean,
      default: true
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    },
    meta: {
      type: Object,
      default: null
    }
  },
  data: () => ({
    price: 0,
    discount: 0,
    quantity: 1
  }),
  computed: {
    sum() {
      return (this.price - this.discount) * this.quantity;
    }
  },
  watch: {
    "entity.price"() {
      this.rebuild();
    },
    "entity.quantity"() {
      this.rebuild();
    },
    "entity.discount"() {
      this.rebuild();
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.discount = this.discountable ? this.entity.discount : 0;
      this.price = this.entity.price + this.discount;
      this.quantity = this.entity.quantity;
    },
    getCurrency(value) {
      return Strings$1.currency(value);
    },
    onQuantityChange(val) {
      this.quantity = this.parseValue(val, 1);
    },
    parseValue(val, def) {
      var parsedValue = parseFloat(val);
      if (isNaN(parsedValue)) {
        return def;
      }
      return parsedValue;
    }
  }
};
const __cssModules$5 = {};
var component$5 = normalizeComponent(script$5, render$5, staticRenderFns$5, false, injectStyles$5, null, null, null);
function injectStyles$5(context) {
  for (let o in __cssModules$5) {
    this[o] = __cssModules$5[o];
  }
}
component$5.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/order-item-pricing.vue";
var OrderItemPricing = component$5.exports;
const editor$1 = new Editor("commerce.order-item-customization", "@shop.order.customization.");
editor$1.field("name").text(80).required();
editor$1.field("description").when((x) => x.description).output();
editor$1.field("value").text(40);
editor$1.field("position").number();
editor$1.field("price", {hideLabel: true}).custom(OrderItemPricing, {discountable: false});
var render$4 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-productpicker-shim"}, [_c("product-picker", _vm._b({attrs: {value: _vm.model}, on: {input: function($event) {
    return _vm.onChange($event);
  }}}, "product-picker", {limit: _vm.limit, disabled: _vm.disabled, root: _vm.root, channel: _vm.channel, channelPickerDisabled: _vm.channelPickerDisabled, options: _vm.options}, false))], 1);
};
var staticRenderFns$4 = [];
const script$4 = {
  name: "uiProductpickerShim",
  props: {
    value: {
      type: String,
      default: null
    },
    entity: {
      type: Object,
      required: true
    },
    limit: {
      type: Number,
      default: 1
    },
    disabled: {
      type: Boolean,
      default: false
    },
    root: {
      type: String,
      default: null
    },
    channelPickerDisabled: {
      type: Boolean,
      default: false
    },
    options: {
      type: Object,
      default: () => {
      }
    },
    config: Object
  },
  components: {ProductPicker: ProductPicker$1},
  data: () => ({
    channel: null,
    model: null
  }),
  watch: {
    entity: {
      deep: true,
      handler() {
        this.rebuild();
      }
    }
  },
  mounted() {
    this.rebuild();
  },
  methods: {
    rebuild() {
      this.channel = this.entity.channelId;
      this.model = this.entity.productId ? {
        id: this.entity.productId,
        channelId: this.channel,
        variantId: this.entity.variantId
      } : null;
    },
    onChange(value) {
      if (!value) {
        this.entity.productId = null;
        this.entity.variantId = null;
      } else {
        this.entity.productId = value.id;
        this.entity.variantId = value.variantId;
      }
      this.$emit("change", value.id);
      this.$emit("input", value.id);
    }
  }
};
const __cssModules$4 = {};
var component$4 = normalizeComponent(script$4, render$4, staticRenderFns$4, false, injectStyles$4, null, null, null);
function injectStyles$4(context) {
  for (let o in __cssModules$4) {
    this[o] = __cssModules$4[o];
  }
}
component$4.options.__file = "../zero.Commerce/Plugin/pickers/product/picker-shim.vue";
var ProductPicker = component$4.exports;
const editor = new Editor("commerce.order-item", "@shop.order.item.");
const general = editor.tab("general", "@ui.tab_general");
const customizations = editor.tab("customizations", "@shop.order.item.tab_customizations", (x) => x.customizations.length);
general.field("productId").custom(ProductPicker, {limit: 1, channelPickerDisabled: true});
general.field("name").text(120).required();
general.field("description").text(240);
general.field("price", {hideLabel: true}).custom(OrderItemPricing);
customizations.field("customizations", {hideLabel: true}).nested(editor$1, {
  limit: 10,
  title: "@shop.order.customization.title",
  itemLabel: (x) => x.quantity + " \xD7 " + x.name,
  itemDescription: (x) => {
    let props = [];
    props.push({key: "Price", value: Strings$1.currency(x.price)});
    if (x.value) {
      props.push({key: "Value", value: x.value});
    }
    return props.map((x2) => x2.key + ": " + x2.value).join(", ");
  },
  itemIcon: "fth-pencil",
  template: {
    id: null,
    customizationId: null,
    position: 0,
    price: 0,
    quantity: 1,
    name: null,
    description: null,
    value: null
  }
});
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "ui-order-edit-item", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: _vm.config.title, "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", submit: true, label: "@ui.confirm", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), _c("ui-editor", {attrs: {config: _vm.editorConfig, disabled: _vm.disabled}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1)];
  }}])});
};
var staticRenderFns$3 = [];
const script$3 = {
  props: {
    config: Object
  },
  components: {ProductPicker: ProductPicker$1, UiEditor},
  data: () => ({
    loading: false,
    disabled: false,
    model: {},
    types: [
      {label: "@shop.order.item_type.product", value: "product"},
      {label: "@shop.order.item_type.custom", value: "custom"}
    ],
    editorConfig: editor,
    product: null,
    channelId: null
  }),
  watch: {
    "model.productId"() {
      this.rebuild();
    },
    "model.variantId"() {
      this.rebuild();
    }
  },
  methods: {
    onLoad() {
      this.model = JSON.parse(JSON.stringify(this.config.model));
      this.channelId = this.config.channelId;
      this.model.channelId = this.channelId;
      this.$nextTick(() => this.loading = false);
    },
    rebuild() {
      if (this.loading) {
        return;
      }
    },
    onSubmit() {
      this.config.confirm(this.model);
    }
  }
};
const __cssModules$3 = {};
var component$3 = normalizeComponent(script$3, render$3, staticRenderFns$3, false, injectStyles$3, null, null, null);
function injectStyles$3(context) {
  for (let o in __cssModules$3) {
    this[o] = __cssModules$3[o];
  }
}
component$3.options.__file = "../zero.Commerce/Plugin/pages/orders/overlays/edit-item.vue";
var EditItemOverlay = component$3.exports;
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-item is-handle"}, [_c("div", {staticClass: "ui-table-row"}, [_c("div", {staticClass: "ui-table-cell is-image"}, [_vm.image ? _c("img", {staticClass: "ui-table-field-image", attrs: {src: _vm.image}}) : _vm._e()]), _c("div", {staticClass: "ui-table-cell is-name"}, [_vm.meta ? _c("button", {staticClass: "ui-silent-link", attrs: {type: "button"}, on: {click: function($event) {
    return _vm.preview(_vm.item);
  }}}, [_c("strong", [_vm._v(_vm._s(_vm.item.name))])]) : _c("strong", [_vm._v(_vm._s(_vm.item.name))]), _c("br"), _c("span", {staticClass: "-minor"}, [_vm._v(_vm._s(_vm.item.description))])]), _c("div", {staticClass: "ui-table-cell is-short is-price"}, [_vm.item.discount !== 0 ? _c("s", {staticClass: "-price -strike", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.item.price + _vm.item.discount))}}) : _vm._e(), _c("span", {staticClass: "-price", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.item.price))}})]), _vm.hasDeliveries ? _c("div", {staticClass: "ui-table-cell is-short is-quantity"}, [_c("span", {staticClass: "-minor"}, [_vm._v(_vm._s(_vm.delivered) + " /")]), _vm._v(" " + _vm._s(_vm.item.quantity) + " ")]) : _c("div", {staticClass: "ui-table-cell is-short is-quantity"}, [_vm._v(" " + _vm._s(_vm.item.quantity) + " ")]), _vm.displayCustomizations ? _c("div", {staticClass: "ui-table-cell is-xshort is-price"}, [_vm.item.customizations.length ? _c("span", {staticClass: "-price", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.customizationSum))}}) : _c("span", [_vm._v("-")])]) : _vm._e(), _c("div", {staticClass: "ui-table-cell is-short is-last", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.item.price * _vm.item.quantity + _vm.customizationSum))}}), _c("div", {staticClass: "ui-table-cell is-actions"}, [!_vm.disabled ? _c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
    return [_c("ui-icon-button", {directives: [{name: "localize", rawName: "v-localize:title", value: "@ui.actions", expression: "'@ui.actions'", arg: "title"}], attrs: {type: "blank", icon: "fth-more-vertical"}})];
  }, proxy: true}], null, false, 4003495296)}, [_c("ui-dropdown-button", {attrs: {label: "@ui.edit.title", icon: "fth-edit-2"}, on: {click: function($event) {
    return _vm.editItem(_vm.item);
  }}}), _c("ui-dropdown-button", {attrs: {label: "@ui.add", icon: "fth-plus"}}), _c("ui-dropdown-button", {attrs: {label: "@ui.remove", negative: true, icon: "fth-trash", confirm: true}, on: {click: function($event) {
    return _vm.$emit("remove", _vm.item);
  }}})], 1) : _vm._e()], 1)]), _vm._e()]);
};
var staticRenderFns$2 = [];
var item_vue_vue_type_style_index_0_lang = ".shop-order-item-stock {\n  margin-left: 5px;\n}\n.shop-order-item-customizations {\n  margin-top: -12px;\n  margin-bottom: 10px;\n}\n.shop-order-item-customizations-headline {\n  display: none;\n  padding-left: 100px;\n  font-size: var(--font-size-s);\n  color: var(--color-text);\n  font-weight: 700;\n  margin-bottom: 6px;\n}\n.shop-order-item-customization {\n  border-bottom: none !important;\n}\n.shop-order-item-customization .ui-table-cell {\n  min-height: 0 !important;\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n  padding: 0 20px 0 20px;\n}\n.shop-order-item .is-quantity .-minor {\n  font-size: var(--font-size);\n  margin-right: 0.2em;\n}";
const script$2 = {
  props: {
    item: {
      type: Object,
      default: () => {
      }
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    },
    channel: {
      type: String,
      default: null
    },
    meta: {
      type: Object,
      default: null
    },
    entity: {
      type: Object,
      default: null
    },
    displayCustomizations: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    image() {
      return this.meta && this.meta.imageId ? MediaApi.getImageSource(this.meta.imageId) : null;
    },
    stockCount() {
      return !this.meta ? 0 : reduce(this.meta.stock, (memo, obj) => memo + obj.count, 0);
    },
    delivered() {
      let count = 0;
      if (!this.item || !this.entity || !this.entity.shipping) {
        return count;
      }
      if (this.entity.shipping.deliveries.find((x) => x.items.length < 1)) {
        return this.item.quantity;
      }
      this.entity.shipping.deliveries.forEach((delivery) => {
        let item = delivery.items.find((i) => i.id == this.item.id);
        count += item ? item.quantity : 0;
      });
      return count;
    },
    hasDeliveries() {
      return this.entity && this.entity.shipping && this.entity.shipping.deliveries.length;
    },
    customizationSum() {
      return this.item.customizations.reduce((acc, x) => acc += x.quantity * x.price, 0);
    }
  },
  methods: {
    getCurrency(value) {
      return Strings$1.currency(value);
    },
    preview(item) {
      return Overlay.open({
        title: item.name,
        closeLabel: "@ui.close",
        component: ProductPreviewOverlay,
        id: item.productId,
        variantId: item.variantId,
        width: 1020
      });
    },
    editItem(item) {
      return Overlay.open({
        title: "@shop.order.item.title",
        component: EditItemOverlay,
        display: "editor",
        model: item,
        channelId: this.channel,
        width: 820
      }).then((res) => {
        this.$emit("change", res);
      });
    }
  }
};
const __cssModules$2 = {};
var component$2 = normalizeComponent(script$2, render$2, staticRenderFns$2, false, injectStyles$2, null, null, null);
function injectStyles$2(context) {
  for (let o in __cssModules$2) {
    this[o] = __cssModules$2[o];
  }
}
component$2.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/item.vue";
var OrderItem = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "shop-order-items-outer"}, [_c("div", {staticClass: "ui-box"}, [_c("div", {staticClass: "shop-order-items ui-table"}, [_vm.value.items.length ? _c("header", {staticClass: "ui-table-row ui-table-head"}, [_c("div", {staticClass: "ui-table-cell is-image"}), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.item", expression: "'@shop.order.items.item'"}], staticClass: "ui-table-cell"}), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.price", expression: "'@shop.order.items.price'"}], staticClass: "ui-table-cell is-short"}), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: _vm.hasDeliveries ? "@shop.order.items.shipped" : "@shop.order.items.quantity", expression: "hasDeliveries ? '@shop.order.items.shipped' : '@shop.order.items.quantity'"}], staticClass: "ui-table-cell is-short"}), _vm.hasCustomizations ? _c("div", {directives: [{name: "localize", rawName: "v-localize:title", value: "@shop.order.items.customizations", expression: "'@shop.order.items.customizations'", arg: "title"}], staticClass: "ui-table-cell is-xshort"}, [_c("ui-icon", {attrs: {symbol: "fth-droplets"}})], 1) : _vm._e(), _c("div", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.itemTotal", expression: "'@shop.order.items.itemTotal'"}], staticClass: "ui-table-cell is-short is-last"}), _c("div", {staticClass: "ui-table-cell is-actions"})]) : _vm._e(), _c("main", {directives: [{name: "sortable", rawName: "v-sortable", value: {handle: ".is-handle", onUpdate: _vm.onSortingUpdated}, expression: "{ handle: '.is-handle', onUpdate: onSortingUpdated }"}], staticClass: "shop-order-items-main"}, [_vm._l(_vm.value.items, function(item) {
    return _c("order-item", {key: item.id, attrs: {item, entity: _vm.value, channel: _vm.value.channelId, meta: _vm.productMeta[item.productId + ":" + item.variantId], disabled: _vm.disabled, "display-customizations": _vm.hasCustomizations}, on: {remove: function($event) {
      return _vm.removeItem(item);
    }, change: function($event) {
      return _vm.changeItem($event, item);
    }}});
  }), !_vm.value.items.length ? _c("div", {staticClass: "shop-order-items-empty"}, [_c("ui-button", {attrs: {type: "light", label: "@shop.order.items.addItemFromEmpty", icon: "fth-plus"}})], 1) : _vm._e()], 2)])]), _c("div", {staticClass: "ui-box is-connected"}, [_c("div", {staticClass: "shop-order-summary"}, [_c("div", {staticClass: "shop-order-summary-item"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.subtotal", expression: "'@shop.order.items.subtotal'"}], staticClass: "-key"}), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.subtotal))}})]), _vm.hasCustomizations ? _c("div", {staticClass: "shop-order-summary-item"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.customizations", expression: "'@shop.order.items.customizations'"}], staticClass: "-key"}), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.customizations))}})]) : _vm._e(), _vm.value.shipping ? _c("div", {staticClass: "shop-order-summary-item"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.shipping", expression: "'@shop.order.items.shipping'"}], staticClass: "-key"}), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.shipping))}})]) : _vm._e(), _vm._l(_vm.promotions, function(promotion) {
    return promotion.price !== 0 ? _c("div", {staticClass: "shop-order-summary-item", attrs: {title: promotion.description}}, [_c("span", {staticClass: "-key"}, [_vm._v(_vm._s(promotion.name))]), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(promotion.price * -1))}})]) : _vm._e();
  }), _vm.payableTotal > 0 ? _c("div", {staticClass: "shop-order-summary-item is-bold"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.totalNet", expression: "'@shop.order.items.totalNet'"}], staticClass: "-key"}), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.payableTotal - _vm.tax))}})]) : _vm._e(), _vm._l(_vm.taxes, function(taxRate) {
    return _vm.payableTotal > 0 && taxRate.rate !== 0 ? _c("div", {staticClass: "shop-order-summary-item"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@shop.order.items.taxPercent", tokens: {tax: taxRate.rate}}, expression: "{ key: '@shop.order.items.taxPercent', tokens: { tax: taxRate.rate } }"}], staticClass: "-key"}), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(taxRate.value))}})]) : _vm._e();
  }), _c("div", {staticClass: "shop-order-summary-item is-bold is-summary"}, [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.items.total", expression: "'@shop.order.items.total'"}], staticClass: "-key"}), _c("span", {staticClass: "-value", domProps: {innerHTML: _vm._s(_vm.getCurrency(_vm.payableTotal))}})])], 2)])]);
};
var staticRenderFns$1 = [];
var items_vue_vue_type_style_index_0_lang = ".shop-order-items-outer .ui-box:first-child {\n  padding: 0;\n  margin-top: 0;\n}\n.shop-order-items.ui-table {\n  background: transparent;\n  box-shadow: none;\n}\n.shop-order-items .ui-table-head {\n  background: var(--color-table-head);\n  border-top-left-radius: var(--radius);\n  border-top-right-radius: var(--radius);\n  /*.ui-table-cell\n  {\n    border-left: none;\n  }*/\n}\n.shop-order-items .ui-table-row:not(.ui-table-head) .ui-table-cell {\n  min-height: 74px;\n}\n.shop-order-items .ui-table-cell.is-image {\n  flex: 0 1 80px;\n  padding: 11px 12px 10px 20px;\n  justify-content: center;\n}\n.shop-order-items .ui-table-cell.is-image img {\n  max-height: 48px;\n  max-width: 40px;\n}\n.shop-order-items .ui-table-cell.is-actions {\n  flex: 0 0 32px;\n  justify-content: center;\n  padding: 0;\n  overflow: visible;\n}\n.shop-order-items .ui-table-cell.is-actions .ui-dropdown-container {\n  display: flex;\n  align-items: stretch;\n  height: 100%;\n}\n.shop-order-items .ui-table-cell.is-actions .ui-icon-button {\n  width: 30px;\n  height: 100%;\n  background: var(--color-box);\n  border-radius: 0 !important;\n}\n.shop-order-items .ui-table-cell.is-actions .ui-icon-button .ui-button-icon {\n  color: var(--color-text-dim);\n}\n.shop-order-items .ui-table-cell.is-actions .ui-icon-button:hover .ui-button-icon {\n  color: var(--color-text);\n}\n.shop-order-items .ui-table-cell.is-short {\n  flex: 0 1 140px;\n  justify-content: flex-end;\n}\n.shop-order-items .ui-table-cell.is-xshort {\n  flex: 0 1 120px;\n  justify-content: flex-end;\n}\n.shop-order-items .ui-table-cell.is-price {\n  display: inline-flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: flex-end;\n}\n.shop-order-items .ui-table-cell.is-price s {\n  display: inline-block;\n  font-size: var(--font-size-s);\n  color: var(--color-text-dim);\n}\n.shop-order-items .ui-table-cell.is-name {\n  display: inline-flex;\n  flex-direction: column;\n  justify-content: center;\n  align-items: flex-start;\n}\n.shop-order-items .ui-table-cell.is-name .-minor {\n  display: inline-block;\n  font-size: var(--font-size-s);\n  margin-top: 2px;\n}\n.shop-order-items-empty {\n  padding: var(--padding);\n  display: flex;\n  justify-content: flex-start;\n}\n.shop-order-item + .shop-order-item {\n  border-top: 1px solid var(--color-table-line-horizontal);\n}\n.shop-order-item .ui-table-row {\n  border-bottom: none;\n}\n.shop-order-summary {\n  margin-right: 20px;\n}\n.shop-order-summary-item {\n  display: grid;\n  grid-template-columns: 1fr 120px;\n  justify-content: flex-end;\n  align-items: center;\n  text-align: right;\n}\n.shop-order-summary-item .-key {\n  padding-right: 20px;\n}\n.shop-order-summary-item.is-bold {\n  font-weight: bold;\n}\n.shop-order-summary-item + .shop-order-summary-item {\n  margin-top: 15px;\n}\n.shop-order-summary-item.is-summary .-value {\n  font-size: 22px;\n}";
const script$1 = {
  props: {
    value: {
      type: Object,
      default: () => {
      }
    },
    config: Object,
    disabled: {
      type: Boolean,
      default: false
    },
    productMeta: {
      type: Object,
      default: () => {
      }
    }
  },
  data: () => ({
    subtotal: 0,
    shipping: 0,
    tax: 0,
    hasCustomizations: false,
    customizations: 0,
    taxes: [],
    promotions: [],
    promotionsSum: 0,
    total: 0,
    payableTotal: 0
  }),
  components: {OrderItem},
  watch: {
    value: {
      deep: true,
      handler() {
        this.calculateSums();
      }
    }
  },
  computed: {
    hasDeliveries() {
      return this.value && this.value.shipping && this.value.shipping.deliveries.length;
    }
  },
  mounted() {
    this.calculateSums();
  },
  methods: {
    calculateSums() {
      this.subtotal = 0;
      this.tax = 0;
      this.taxes = [];
      this.promotions = this.value.promotions;
      this.total = 0;
      this.payableTotal = 0;
      this.customizations = 0;
      this.promotionsSum = 0;
      this.hasCustomizations = false;
      this.shipping = this.value.shipping != null ? this.value.shipping.price : 0;
      this.promotions.forEach((p) => {
        this.promotionsSum += p.price;
      });
      let handleItem = (x) => {
        let customizations2 = 0;
        x.customizations.forEach((c) => {
          this.hasCustomizations = true;
          customizations2 += c.price * c.quantity;
        });
        let sum = x.price * x.quantity + customizations2;
        this.subtotal += sum - customizations2;
        this.customizations += customizations2;
        let tax = sum - sum / ((100 + x.taxRate) / 100);
        let taxRate = this.taxes.find((t) => t.rate === x.taxRate);
        if (!taxRate) {
          taxRate = {
            rate: x.taxRate,
            value: tax
          };
          this.taxes.push(taxRate);
        } else {
          taxRate.value += tax;
        }
        this.tax += tax;
      };
      this.value.items.forEach((x) => handleItem(x));
      if (this.shipping !== 0) {
        this.taxes.forEach((x) => {
          let percent = x.value / this.tax * 100;
          let shippingPortion = this.shipping / 100 * percent;
          let shippingTaxPortion = shippingPortion - shippingPortion / ((100 + x.rate) / 100);
          x.value += shippingTaxPortion;
        });
      }
      this.total = this.subtotal + this.customizations + this.shipping;
      this.payableTotal = this.total - this.promotionsSum;
      let pricePercentage = this.payableTotal / this.total;
      this.tax = 0;
      this.taxes.forEach((x) => {
        x.value = x.value * pricePercentage;
        this.tax += x.value;
      });
    },
    getCurrency(value) {
      return Strings$1.currency(value);
    },
    removeItem(item) {
      const index = this.value.items.indexOf(item);
      this.value.items.splice(index, 1);
    },
    changeItem(newItem, oldItem) {
      const index = this.value.items.indexOf(oldItem);
      this.value.items.splice(index, 1, newItem);
    },
    onSortingUpdated(ev) {
      console.info("sort");
      this.value.items = Arrays.move(this.value.items, ev.oldIndex, ev.newIndex);
      let sort = 0;
      this.value.items.forEach((x) => x.sort = sort += 10);
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
component$1.options.__file = "../zero.Commerce/Plugin/pages/orders/partials/items.vue";
var OrderItems = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-order", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-header-bar", {staticClass: "ui-form-header", attrs: {"back-button": true, title: "#" + _vm.model.number, "title-empty": "@shop.order.name", prefix: ["@shop.order.list", _vm.channel ? _vm.channel.name : null]}}, [_c("div", {staticClass: "ui-form-header-aside"}, [_c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", icon: "fth-more-horizontal"}})];
    }, proxy: true}], null, true)}, [_c("ui-dropdown-button", {attrs: {label: "Delete", icon: "fth-trash", disabled: _vm.disabled || !_vm.meta.canDelete}, on: {click: _vm.onDelete}})], 1), !_vm.disabled ? _c("ui-button", {attrs: {submit: true, type: "primary", label: "@ui.save", state: form.state}}) : _vm._e()], 1)]), _c("div", {staticClass: "shop-order-container"}, [_c("div", {staticClass: "shop-order-main"}, [_c("div", {staticClass: "ui-box is-top"}, [_vm.states.length ? _c("order-states", {attrs: {disabled: _vm.disabled, "order-states": _vm.states, meta: _vm.productMeta}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}) : _vm._e()], 1), _c("order-items", {attrs: {disabled: _vm.disabled, "product-meta": _vm.productMeta}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1), _c("aside", {staticClass: "shop-order-tabs"}, [_c("div", {staticClass: "ui-box"}, [_c("order-addresses", {attrs: {disabled: _vm.disabled}, on: {address: _vm.onAddressChange, shipping: _vm.onShippingChange, customer: _vm.onCustomerChange}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1), _c("div", {staticClass: "ui-box is-light shop-order-vertical-props"}, [_c("ui-property", {attrs: {label: "@shop.order.fields.channel", vertical: false, "is-text": true}}, [_vm.channel ? _c("router-link", {staticClass: "ui-link", attrs: {to: {name: "commerce-channels-edit", params: {id: _vm.model.channelId}}}}, [_vm._v(_vm._s(_vm.channel.name))]) : [_vm._v(" " + _vm._s(_vm.model.channelName) + " ")]], 2), _vm.language ? _c("ui-property", {attrs: {label: "@shop.order.fields.language", vertical: false, "is-text": true}}, [_c("router-link", {staticClass: "ui-link", attrs: {to: {name: "languages-edit", params: {id: _vm.language.id}}}}, [_vm._v(_vm._s(_vm.language.name))])], 1) : _vm._e(), _vm.currency ? _c("ui-property", {attrs: {label: "@shop.order.fields.currency", vertical: false, "is-text": true}}, [_vm.currency ? _c("router-link", {staticClass: "ui-link", attrs: {to: {name: "commerce-currencies-edit", params: {id: _vm.currency.id}}}}, [_vm._v(_vm._s(_vm.model.currency.name) + " (" + _vm._s(_vm.model.currency.symbol) + ")")]) : [_vm._v(" " + _vm._s(_vm.model.currency.name) + " (" + _vm._s(_vm.model.currency.symbol) + ") ")]], 2) : _vm._e(), _vm.model.id ? _c("ui-property", {attrs: {label: "@ui.createdDate", vertical: false, "is-text": true}}, [_c("ui-date", {attrs: {format: "long"}, model: {value: _vm.model.createdDate, callback: function($$v) {
      _vm.$set(_vm.model, "createdDate", $$v);
    }, expression: "model.createdDate"}})], 1) : _vm._e(), _vm.model.lastModifiedDate ? _c("ui-property", {attrs: {label: "@ui.modifiedDate", vertical: false, "is-text": true}}, [_c("ui-date", {attrs: {format: "long"}, model: {value: _vm.model.lastModifiedDate, callback: function($$v) {
      _vm.$set(_vm.model, "lastModifiedDate", $$v);
    }, expression: "model.lastModifiedDate"}})], 1) : _vm._e(), !_vm.disabled ? _c("ui-property", {attrs: {vertical: false, "is-text": true}}, [_c("div", {staticClass: "editor-aside-links"}, [_c("button", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.meta.editLink", expression: "'@shop.order.meta.editLink'"}], staticClass: "ui-link", attrs: {type: "button"}, on: {click: _vm.editMeta}})])]) : _vm._e()], 1)])])];
  }}])});
};
var staticRenderFns = [];
var order_vue_vue_type_style_index_0_lang = "/*.shop-order-main-outer\n  {\n\n  }\n*/\n.shop-order-container {\n  display: grid;\n  grid-template-columns: 1fr 380px var(--padding) !important;\n}\n.shop-order-main .ui-tabs-list {\n  display: none;\n}\n.shop-order-main .ui-tab, .shop-order-main .ui-box.is-top {\n  margin-top: 0;\n}\n.shop-order-main > .ui-box.is-top {\n  margin-bottom: var(--padding-s);\n}\nh2.shop-order-top-name {\n  color: var(--color-text);\n  margin-bottom: 7px !important;\n}\n.shop-order-aside {\n  margin-top: 0;\n}\n.shop-order-aside-line {\n  border-bottom-color: var(--color-line-onbg);\n}\n.shop-order-tabs .ui-box {\n  margin: 0;\n}\n.shop-order-tabs .ui-box:first-child {\n  border-bottom-left-radius: 0;\n  border-bottom-right-radius: 0;\n}\n.shop-order-tabs .ui-box + .shop-order-vertical-props {\n  border-top: 1px solid var(--color-line);\n  border-top-left-radius: 0;\n  border-top-right-radius: 0;\n}\n.shop-order-tabs .ui-box.shop-order-vertical-props .ui-property + .ui-property {\n  margin-top: var(--padding-s);\n  padding-top: 0;\n  border-top: none;\n}\n.shop-order-tabs .ui-tabs-list {\n  padding: 0;\n  margin-bottom: 0;\n}\n.shop-order-wrap-text {\n  line-height: 1.4;\n}\n.shop-order-wrap-text.no-wrap {\n  white-space: nowrap;\n  overflow: hidden;\n  text-overflow: ellipsis;\n}\n.shop-order-top {\n  display: grid;\n  grid-template-columns: auto 1fr auto;\n  grid-gap: var(--padding-m);\n  align-items: center;\n}\n.shop-order-top-icon {\n  display: inline-block;\n  float: left;\n  width: 70px;\n  height: 70px;\n  line-height: 74px;\n  font-size: 24px;\n  border-radius: var(--radius);\n  background: var(--color-button-light);\n  color: var(--color-text);\n  text-align: center;\n}\nh2.shop-order-top-name {\n  margin: 0 0 5px;\n  font-size: var(--font-size-l);\n  font-weight: bold;\n}\n.shop-order-top-date {\n  color: var(--color-text-dim);\n}\n.shop-order-top-line {\n  margin: var(--padding) 0;\n}\n.shop-order-assignedto .ui-property-content {\n  padding-top: 6px;\n}\n.shop-order-assignedto .ui-pick-overlay .ui-dropdown {\n  left: auto;\n  right: -48px;\n  width: 360px;\n}\n.shop-order-tabs .ui-property-content .-minor {\n  font-weight: 400;\n  color: var(--color-text-dim-one);\n  font-size: var(--font-size-s);\n  line-height: 1.5;\n}";
const script = {
  props: ["id"],
  components: {UiEditor, OrderAddresses, OrderStates, OrderItems, OrderDocuments, OrderAddress},
  data: () => ({
    meta: {},
    channel: null,
    isCustomerNoteToggled: false,
    model: {
      name: null,
      address: {},
      items: [],
      promotions: [],
      shipping: null,
      documents: []
    },
    language: null,
    currency: null,
    states: [],
    productMeta: {},
    route: "commerce-orders-edit",
    disabled: false
  }),
  computed: {
    noteCount() {
      return this.model.internalNote && this.model.customerNote ? 2 : this.model.internalNote || this.model.customerNote ? 1 : 0;
    }
  },
  methods: {
    onLoad(form) {
      form.load(!this.id ? OrdersApi.getEmpty() : OrdersApi.getById(this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
        this.channel = response.channel;
        this.states = response.states;
        this.productMeta = response.productMeta;
        this.language = response.language;
        this.currency = response.currency;
      });
    },
    onSubmit(form) {
      return form.handle(OrdersApi.save(this.model)).then((res) => {
        this.model = res.model;
      });
    },
    onDelete() {
      this.$refs.form.onDelete(OrdersApi.delete.bind(this, this.id));
    },
    onAddressChange(address) {
      this.model.address = address;
    },
    onShippingChange(data) {
      this.model.shipping = data;
    },
    onCustomerChange(data) {
      this.model.customer = data;
    },
    editInternalNote(note) {
      console.info(note);
    },
    editMeta() {
      return Overlay.open({
        component: UiEditorOverlay,
        display: "editor",
        editor: "commerce.order-meta",
        title: "@shop.order.meta.headline",
        model: this.model,
        width: 680,
        create: false
      }).then((value) => {
        this.model = value;
      });
    },
    viewRevisions() {
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
component.options.__file = "../zero.Commerce/Plugin/pages/orders/order.vue";
var order = component.exports;
var order$1 = /* @__PURE__ */ Object.freeze({
  __proto__: null,
  [Symbol.toStringTag]: "Module",
  default: order
});
export {OrderAddresses as O, OrderStates as a, OrderItems as b, OrderDocuments as c, OrderDeliveries as d, OrderAddress as e, order$1 as o};

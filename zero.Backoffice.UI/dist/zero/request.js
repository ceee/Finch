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
import {g as get, n as normalizeComponent, a5 as RequestsApi, U as UiEditor, S as Strings, b as MediaApi, O as Overlay, z as UiEditorOverlay} from "./index.js";
import {O as OrderAddresses, a as OrderStates, b as OrderItems, c as OrderDocuments, d as OrderDeliveries, e as OrderAddress} from "./order.js";
import {e as extend} from "./vendor.js";
const base = "teamSpace/";
var TeamSpaceApi = {
  getPreviews: async (ids, config) => await get(base + "getPreviews", __assign(__assign({}, config), {params: {ids}})),
  getForPicker: async (config) => await get(base + "getForPicker", __assign({}, config))
};
var render$3 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("div", {staticClass: "laola-teampicker", class: {"is-disabled": _vm.disabled}}, [_c("ui-pick", {attrs: {config: _vm.pickerConfig, value: _vm.value, disabled: _vm.disabled}, on: {input: _vm.onChange}})], 1);
};
var staticRenderFns$3 = [];
var teampicker_vue_vue_type_style_index_0_lang = ".laola-teampicker .ui-select-button-icon.is-image, .laola-teampicker .ui-select-button-icon {\n  padding: 0;\n  border-radius: 50px;\n}\n.laola-teampicker .ui-select-button-icon.is-image img, .laola-teampicker .ui-select-button-icon img {\n  border-radius: 50px;\n}";
const script$3 = {
  name: "laolaTeamPicker",
  props: {
    value: {
      type: [String, Array],
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
    previews: [],
    pickerConfig: {}
  }),
  created() {
    this.pickerConfig = extend({
      scope: "team",
      items: TeamSpaceApi.getForPicker,
      previews: TeamSpaceApi.getPreviews,
      limit: this.limit,
      multiple: this.limit > 1,
      preview: {
        enabled: true,
        iconAsImage: true
      }
    }, this.options);
  },
  methods: {
    onChange(value) {
      this.$emit("input", value);
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
component$3.options.__file = "../../Laola/Laola.Backoffice/Plugin/components/teampicker.vue";
var TeamPicker = component$3.exports;
var render$2 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "laola-offer-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: "Send offer", "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", submit: true, label: "Save & send", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-box"}, [_c("ui-property", {staticClass: "shop-order-assignedto", attrs: {label: "From", "is-text": true, vertical: true}}, [_c("team-picker", {model: {value: _vm.model.teamMemberId, callback: function($$v) {
      _vm.$set(_vm.model, "teamMemberId", $$v);
    }, expression: "model.teamMemberId"}})], 1), _c("ui-property", {attrs: {label: "Recipient", vertical: true, "is-text": true}}, [_vm._v(" " + _vm._s(_vm.config.email) + " ")]), _c("ui-property", {attrs: {label: "Message", vertical: true}}, [_c("textarea", {directives: [{name: "model", rawName: "v-model", value: _vm.model.message, expression: "model.message"}], staticClass: "ui-textarea", attrs: {rows: "3"}, domProps: {value: _vm.model.message}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "message", $event.target.value);
    }}})]), _c("ui-property", {attrs: {label: "Reminders", description: "This offer is re-sent in the specified timeframe if it has not been completed yet", vertical: true}}, [_c("ui-select", {attrs: {items: _vm.reminders}, model: {value: _vm.model.reminderConfig, callback: function($$v) {
      _vm.$set(_vm.model, "reminderConfig", $$v);
    }, expression: "model.reminderConfig"}})], 1), _vm.model.reminderConfig === "custom" ? _c("ui-property", {attrs: {label: "Configure reminders", description: "Enter the day numbers (starting today) and separate them with a comma. (e.g. 3,7,12 means after 3, 7 and 12 days). Maximum number of reminders is three.", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.model.reminderCustomConfig, expression: "model.reminderCustomConfig"}], attrs: {type: "text", maxlength: "30"}, domProps: {value: _vm.model.reminderCustomConfig}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.model, "reminderCustomConfig", $event.target.value);
    }}})]) : _vm._e()], 1) : _vm._e(), !_vm.loading ? _c("p", {staticClass: "ui-property-help"}, [_vm._v(" The offer is sent as-is. If you still need to make changes to articles or prices, please close the overlay and continue afterwards. ")]) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$2 = [];
var offer_vue_vue_type_style_index_0_lang = ".laola-offer-overlay .-text {\n  line-height: 1.5;\n  font-size: var(--font-size);\n}";
const script$2 = {
  props: {
    config: Object
  },
  components: {TeamPicker},
  data: () => ({
    disabled: false,
    loading: false,
    state: "default",
    meta: {},
    model: {},
    reminders: [
      {key: "none", value: "None"},
      {key: "perThreeDays", value: "After 6, 9 and 15 days"},
      {key: "perWeek", value: "After 1, 2 and 4 weeks"},
      {key: "custom", value: "Custom..."}
    ]
  }),
  created() {
  },
  methods: {
    onLoad(form) {
      this.model = JSON.parse(JSON.stringify(this.config.model));
    },
    onSubmit(form) {
      var instance = this;
      form.setState("loading");
      this.config.confirm({
        close() {
          instance.config.close();
        },
        model: this.model
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
component$2.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/requests/overlays/offer.vue";
var OfferOverlay = component$2.exports;
var render$1 = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-overlay-editor", {staticClass: "laola-offer-overlay", scopedSlots: _vm._u([{key: "header", fn: function() {
      return [_c("ui-header-bar", {attrs: {title: "Send mail", "back-button": false, "close-button": true}})];
    }, proxy: true}, {key: "footer", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", label: "@ui.close"}, on: {click: _vm.config.hide}}), _c("ui-button", {attrs: {type: "primary", submit: true, label: "Send", state: form.state, disabled: _vm.loading || _vm.disabled}})];
    }, proxy: true}], null, true)}, [_vm.loading ? _c("ui-loading", {attrs: {"is-big": true}}) : _vm._e(), !_vm.loading ? _c("div", {staticClass: "ui-box"}, [_c("ui-property", {attrs: {label: "Recipient", vertical: true, "is-text": true}}, [_vm._v(" " + _vm._s(_vm.model.customer.email) + " ")]), _c("ui-property", {attrs: {label: "Subject", vertical: true}}, [_c("input", {directives: [{name: "model", rawName: "v-model", value: _vm.value.subject, expression: "value.subject"}], staticClass: "ui-input", attrs: {type: "text", maxlength: "80", readonly: _vm.disabled}, domProps: {value: _vm.value.subject}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.value, "subject", $event.target.value);
    }}})]), _c("ui-property", {attrs: {label: "Message", vertical: true, "is-text": true}}, [_vm.messageBefore ? _c("div", {staticStyle: {"margin-bottom": "8px"}, domProps: {innerHTML: _vm._s(_vm.messageBefore)}}) : _vm._e(), _c("textarea", {directives: [{name: "model", rawName: "v-model", value: _vm.value.message, expression: "value.message"}], staticClass: "ui-textarea", attrs: {rows: "3"}, domProps: {value: _vm.value.message}, on: {input: function($event) {
      if ($event.target.composing) {
        return;
      }
      _vm.$set(_vm.value, "message", $event.target.value);
    }}}), _vm.messageAfter ? _c("div", {staticStyle: {"margin-top": "8px"}, domProps: {innerHTML: _vm._s(_vm.messageAfter)}}) : _vm._e()]), _vm.model.offers.length > 0 ? _c("ui-property", {attrs: {label: "Attach offer", description: "Adds the lastest offer as an attachment", vertical: false}}, [_c("ui-toggle", {model: {value: _vm.value.attachOffer, callback: function($$v) {
      _vm.$set(_vm.value, "attachOffer", $$v);
    }, expression: "value.attachOffer"}})], 1) : _vm._e()], 1) : _vm._e()], 1)];
  }}])});
};
var staticRenderFns$1 = [];
var sendmail_vue_vue_type_style_index_0_lang = ".laola-offer-overlay .-text {\n  line-height: 1.5;\n  font-size: var(--font-size);\n}";
const script$1 = {
  props: {
    config: Object
  },
  components: {TeamPicker},
  data: () => ({
    disabled: false,
    loading: false,
    state: "default",
    meta: {},
    model: {
      customer: {}
    },
    value: {
      orderId: null,
      subject: null,
      message: null,
      attachOffer: false
    },
    messageBefore: null,
    messageAfter: null
  }),
  created() {
  },
  methods: {
    onLoad(form) {
      this.loading = true;
      this.model = JSON.parse(JSON.stringify(this.config.model));
      this.value.orderId = this.model.id;
      RequestsApi.getMessageTemplate(this.model.channelId).then((res) => {
        this.value.subject = res.entity.subject;
        let messageParts = res.entity.body.split("{message}");
        this.messageBefore = messageParts[0];
        this.messageAfter = messageParts[1];
        this.loading = false;
      });
    },
    onSubmit(form) {
      form.setState("loading");
      RequestsApi.sendMessage(this.value).then((res) => {
        form.setState("default");
        this.config.confirm();
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
component$1.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/requests/overlays/sendmail.vue";
var SendMailOverlay = component$1.exports;
var render = function() {
  var _vm = this;
  var _h = _vm.$createElement;
  var _c = _vm._self._c || _h;
  return _c("ui-form", {ref: "form", staticClass: "shop-order laola-request", attrs: {route: _vm.route}, on: {submit: _vm.onSubmit, load: _vm.onLoad}, scopedSlots: _vm._u([{key: "default", fn: function(form) {
    return [_c("ui-header-bar", {staticClass: "ui-form-header", attrs: {"back-button": true, title: "#" + _vm.model.number, "title-empty": "@shop.order.name", prefix: ["@laola.request.list", _vm.channel ? _vm.channel.name : null]}}, [!_vm.disabled ? _c("div", {staticClass: "ui-form-header-aside"}, [_c("ui-dropdown", {attrs: {align: "right"}, scopedSlots: _vm._u([{key: "button", fn: function() {
      return [_c("ui-button", {attrs: {type: "light onbg", icon: "fth-more-horizontal"}})];
    }, proxy: true}], null, true)}, [!_vm.model.isCompleted ? _c("ui-dropdown-button", {attrs: {label: "Send mail", icon: "fth-send"}, on: {click: _vm.sendMail}}) : _vm._e(), _vm.model.isCompleted ? _c("ui-dropdown-button", {attrs: {label: "Reopen", icon: "fth-forward"}, on: {click: function($event) {
      return _vm.toggleCompleted(false);
    }}}) : _vm._e(), _c("ui-dropdown-button", {attrs: {label: "Delete", icon: "fth-trash"}, on: {click: _vm.onDelete}})], 1), !_vm.model.isCompleted ? _c("ui-button", {attrs: {type: "light onbg", label: "Mark as completed"}, on: {click: function($event) {
      return _vm.toggleCompleted(true);
    }}}) : _vm._e(), !_vm.model.isCompleted ? _c("ui-button", {attrs: {type: "light onbg", label: "@ui.save", state: form.state}, on: {click: _vm.save}}) : _vm._e(), !_vm.model.isCompleted ? _c("ui-button", {attrs: {type: "primary", icon: "fth-chevron-right", label: "Send offer"}, on: {click: _vm.createOffer}}) : _vm._e()], 1) : _vm._e()]), _c("div", {staticClass: "shop-order-container"}, [_c("div", {staticClass: "shop-order-main"}, [_vm.offerPreview || _vm.model.customer.note ? _c("div", {staticClass: "ui-box is-top"}, [_vm.offerPreview && _vm.offerPreview.number ? _c("div", {staticClass: "ui-property is-vertical laola-request-offerpreview"}, [_c("label", {staticClass: "ui-property-label"}, [!_vm.offerPreview.member ? _c("span", {staticClass: "-image"}, [_c("ui-icon", {attrs: {symbol: "fth-corner-down-right"}})], 1) : _c("span", {staticClass: "-image"}, [_c("img", {attrs: {src: _vm.offerPreview.member.image, alt: _vm.offerPreview.member.name, title: _vm.offerPreview.member.name}})]), _c("span", [_c("span", {directives: [{name: "localize", rawName: "v-localize", value: {key: "@laola.request.fields.offerPreview", tokens: {number: _vm.offerPreview.number}}, expression: "{ key: '@laola.request.fields.offerPreview', tokens: { number: offerPreview.number }}"}]}), _c("small", [_vm.offerPreview.member ? _c("span", [_vm._v(_vm._s(_vm.offerPreview.member.name) + " \u22C5 ")]) : _vm._e(), _c("ui-date", {attrs: {value: _vm.offerPreview.createdDate, format: "long"}})], 1)]), _vm.offerPreview.documentUrl ? _c("a", {staticClass: "ui-button type-light type-small", attrs: {href: _vm.offerPreview.documentUrl, target: "_blank"}}, [_vm._v("PDF"), _c("ui-icon", {staticClass: "ui-button-icon", attrs: {symbol: "fth-file-text", size: 14}})], 1) : _vm._e()]), _vm.offerPreview.message ? _c("div", {staticClass: "ui-property-content"}, [_c("p", {staticClass: "laola-request-offerpreview-message"}, [_vm._v(_vm._s(_vm.offerPreview.message))])]) : _vm._e()]) : _vm._e(), _vm.model.customer.note ? _c("ui-property", {attrs: {label: "@shop.order.fields.customerNote", "is-text": true}}, [_c("p", {staticClass: "laola-request-offerpreview-message"}, [_vm._v(_vm._s(_vm.model.customer.note))])]) : _vm._e()], 1) : _vm._e(), _c("order-items", {attrs: {disabled: _vm.disabled || _vm.model.isCompleted, "product-meta": _vm.productMeta}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}})], 1), _c("aside", {staticClass: "shop-order-tabs"}, [_c("div", {staticClass: "ui-box"}, [_c("order-addresses", {attrs: {disabled: _vm.disabled || _vm.model.isCompleted}, on: {address: _vm.onAddressChange, shipping: _vm.onShippingChange}, model: {value: _vm.model, callback: function($$v) {
      _vm.model = $$v;
    }, expression: "model"}}), _vm.model.customerClub ? _c("hr") : _vm._e(), _vm.model.customerClub ? _c("ui-property", {attrs: {label: "Club", "is-text": true, vertical: true}}, [_vm._v(" " + _vm._s(_vm.model.customerClub) + " ")]) : _vm._e()], 1), _c("div", {staticClass: "ui-box is-light shop-order-vertical-props"}, [_c("ui-property", {attrs: {label: "@shop.order.fields.channel", vertical: false, "is-text": true}}, [_vm.channel ? _c("router-link", {staticClass: "ui-link", attrs: {to: {name: "commerce-channels-edit", params: {id: _vm.model.channelId}}}}, [_vm._v(_vm._s(_vm.channel.name))]) : [_vm._v(" " + _vm._s(_vm.model.channelName) + " ")]], 2), _vm.language ? _c("ui-property", {attrs: {label: "@shop.order.fields.language", vertical: false, "is-text": true}}, [_c("router-link", {staticClass: "ui-link", attrs: {to: {name: "languages-edit", params: {id: _vm.language.id}}}}, [_vm._v(_vm._s(_vm.language.name))])], 1) : _vm._e(), _vm.currency ? _c("ui-property", {attrs: {label: "@shop.order.fields.currency", vertical: false, "is-text": true}}, [_vm.currency ? _c("router-link", {staticClass: "ui-link", attrs: {to: {name: "commerce-currencies-edit", params: {id: _vm.currency.id}}}}, [_vm._v(_vm._s(_vm.model.currency.name) + " (" + _vm._s(_vm.model.currency.symbol) + ")")]) : [_vm._v(" " + _vm._s(_vm.model.currency.name) + " (" + _vm._s(_vm.model.currency.symbol) + ") ")]], 2) : _vm._e(), _vm.model.id ? _c("ui-property", {attrs: {label: "@ui.createdDate", vertical: false, "is-text": true}}, [_c("ui-date", {attrs: {format: "long"}, model: {value: _vm.model.createdDate, callback: function($$v) {
      _vm.$set(_vm.model, "createdDate", $$v);
    }, expression: "model.createdDate"}})], 1) : _vm._e(), _vm.model.lastModifiedDate ? _c("ui-property", {attrs: {label: "@ui.modifiedDate", vertical: false, "is-text": true}}, [_c("ui-date", {attrs: {format: "long"}, model: {value: _vm.model.lastModifiedDate, callback: function($$v) {
      _vm.$set(_vm.model, "lastModifiedDate", $$v);
    }, expression: "model.lastModifiedDate"}})], 1) : _vm._e(), !_vm.disabled && !_vm.model.isCompleted ? _c("ui-property", {attrs: {vertical: false, "is-text": true}}, [_c("div", {staticClass: "editor-aside-links"}, [_c("button", {directives: [{name: "localize", rawName: "v-localize", value: "@shop.order.meta.editLink", expression: "'@shop.order.meta.editLink'"}], staticClass: "ui-link", attrs: {type: "button"}, on: {click: _vm.editMeta}})])]) : _vm._e()], 1)])])];
  }}])});
};
var staticRenderFns = [];
var request_vue_vue_type_style_index_0_lang = ".laola-request .shop-order-main .ui-tabs-list {\n  display: block;\n  padding-top: 0;\n  margin-bottom: 0;\n}\n.laola-request .ui-table-head {\n  border-top-left-radius: var(--radius);\n  border-top-right-radius: var(--radius);\n}\n.laola-request-top-box {\n  border-bottom-left-radius: 0;\n  border-bottom-right-radius: 0;\n  margin-bottom: 0;\n  border-bottom: 1px solid var(--color-line);\n}\n.laola-request-offer {\n  padding-top: var(--padding);\n  margin-top: var(--padding);\n  border-top: 1px solid var(--color-line);\n}\n.laola-request-offerpreview .ui-property-content {\n  max-width: 100%;\n}\n.laola-request-offerpreview .ui-property-label {\n  display: grid;\n  grid-template-columns: auto minmax(0, 1fr) auto;\n  grid-gap: var(--padding-s);\n  align-items: center;\n}\n.laola-request-offerpreview .ui-property-label .-image {\n  width: 48px;\n  height: 48px;\n  display: inline-flex;\n  justify-content: center;\n  align-items: center;\n  border-radius: 48px;\n  background: var(--color-button-light);\n  border: 1px solid transparent;\n  color: var(--color-text);\n  text-align: center;\n  font-size: 16px;\n  flex-shrink: 0;\n  overflow: hidden;\n}\n.laola-request-offerpreview-message {\n  line-height: 1.4;\n}";
const script = {
  props: ["id"],
  components: {UiEditor, OrderAddresses, OrderStates, OrderItems, OrderDocuments, OrderDeliveries, OrderAddress, TeamPicker},
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
      documents: [],
      offers: []
    },
    language: null,
    currency: null,
    customer: null,
    states: [],
    productMeta: {},
    route: "commerce-requests-edit",
    disabled: false,
    offerPreview: null
  }),
  watch: {
    "model.offers": {
      deep: true,
      handler(val) {
        this.buildOffer();
      }
    }
  },
  computed: {
    noteCount() {
      return this.model.internalNote && this.model.customerNote ? 2 : this.model.internalNote || this.model.customerNote ? 1 : 0;
    }
  },
  mounted() {
    this.buildOffer();
  },
  methods: {
    formatDate(val, format) {
      return Strings.date(val, format);
    },
    buildOffer() {
      let offer = this.model.offers.length ? this.model.offers[this.model.offers.length - 1] : null;
      if (!offer) {
        this.offerPreview = null;
        return;
      }
      let document = this.model.documents.find((x) => x.id == offer.documentId);
      this.offerPreview = __assign(__assign({}, offer), {
        document,
        member: null,
        documentUrl: document ? RequestsApi.getDocumentUrl(document.source, document.securityKey) : null
      });
      if (offer.teamMemberId) {
        TeamSpaceApi.getPreviews([offer.teamMemberId]).then((res) => {
          this.offerPreview.member = res[0].hasError ? null : __assign(__assign({}, res[0]), {image: MediaApi.getImageSource(res[0].icon)});
        });
      }
    },
    onLoad(form) {
      form.load(!this.id ? RequestsApi.getEmpty() : RequestsApi.getByIdAsOrder(this.id)).then((response) => {
        this.disabled = !response.meta.canEdit;
        this.meta = response.meta;
        this.model = response.entity;
        this.channel = response.channel;
        this.states = response.states;
        this.productMeta = response.productMeta;
        this.language = response.language;
        this.currency = response.currency;
        this.customer = response.customer;
      });
    },
    onSubmit(form) {
      return form.handle(RequestsApi.save(this.model)).then((res) => {
        this.model = res.model;
      });
    },
    onDelete() {
      this.$refs.form.onDelete(RequestsApi.delete.bind(this, this.id));
    },
    save() {
      return this.onSubmit(this.$refs.form);
    },
    toggleCompleted(isCompleted) {
      this.model.isCompleted = isCompleted;
      this.save();
    },
    createOffer() {
      var lastTeamMember = this.model.offers.length ? this.model.offers[this.model.offers.length - 1].teamMemberId : null;
      return Overlay.open({
        component: OfferOverlay,
        display: "editor",
        title: "@ui.edit.title",
        model: {
          id: null,
          teamMemberId: lastTeamMember,
          message: null,
          remindersSent: [],
          reminderConfig: "none",
          reminderCustomConfig: null
        },
        autoclose: false,
        email: this.model.customer.email
      }).then((opts) => {
        this.model.offers.push(opts.model);
        this.save().then((res) => {
          opts.close();
        });
      });
    },
    sendMail() {
      return Overlay.open({
        component: SendMailOverlay,
        display: "editor",
        title: "@ui.edit.title",
        model: this.model
      }).then((x) => {
      });
    },
    onAddressChange(address) {
      this.model.address = address;
    },
    onShippingChange(data) {
      this.model.shipping = data;
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
component.options.__file = "../../Laola/Laola.Backoffice/Plugin/pages/requests/request.vue";
var request = component.exports;
export default request;

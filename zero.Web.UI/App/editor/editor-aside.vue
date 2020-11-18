<template>
  <aside v-if="!nested && infos && infos != 'none'" class="editor-infos">
    <component v-if="component" :is="component" :editor="editor" v-model="value" :meta="meta" :disabled="disabled" />
    <slot name="info-boxes"></slot>
    <div class="ui-box editor-active-toggle" v-if="activeToggle" :class="{'is-active': value.isActive }">
      <slot name="settings">
        <ui-property v-if="activeToggle" field="isActive" label="@ui.active" :is-text="true" class="is-toggle">
          <ui-toggle v-model="value.isActive" class="is-primary" :disabled="disabled" />
        </ui-property>
      </slot>
      <slot name="settings-properties"></slot>
    </div>
    <div class="ui-box is-light" v-if="value.id">
      <slot name="infos">
        <ui-property v-if="value.id && value.lastModifiedDate"  field="lastModifiedDate" label="@ui.modifiedDate" :is-text="true">
          <ui-date v-model="value.lastModifiedDate" />
        </ui-property>
        <ui-property v-if="value.id" label="@ui.createdDate" field="createdDate" :is-text="true">
          <ui-date v-model="value.createdDate" />
        </ui-property>
        <slot name="infos-more"></slot>
      </slot>
    </div>
    <slot name="infos-after"></slot>
  </aside>
</template>


<script>
  export default {
    name: 'uiEditorAside',

    inject: [ 'meta' ],

    props: {
      editor: {
        type: Object,
        required: true
      },
      value: {
        type: Object,
        required: true
      },
      infos: {
        type: String,
        default: 'aside'
      },
      activeToggle: {
        type: Boolean,
        default: true
      },
      nested: {
        type: Boolean,
        default: false
      },
      isPage: {
        type: Boolean,
        default: false
      },
      disabled: {
        type: [Boolean, Function],
        default: false
      }
    },

    data: () => ({
      component: null
    }),

    created()
    {
      this.component = zero.overrides['editor-aside'] || null;
    }
  }
</script>
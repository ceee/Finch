<template>
  <aside v-if="!nested && infos && infos != 'none'" class="editor-infos">
    <slot name="info-boxes"></slot>
    <!--<div class="ui-box" v-if="isShared" :class="{'is-active': value.isActive }">
      <div class="editor-global-flag">
        <i class="fth-radio"></i>
        <p>
          <b>This entity is bound to a parent</b> and automatically synchronised.<br>-->
          <!--<a href="/">Edit parent</a>-->
        <!--</p>
      </div>
      <ui-button type="light small" label="Settings" @click="editBlueprint(value.blueprint)" />
      <ui-button type="light small" label="Edit parent" @click="editBlueprint(value.blueprint)" />
    </div>-->
    <div class="ui-box editor-active-toggle" v-if="activeToggle" :class="{'is-active': value.isActive }">
      <slot name="settings">
        <ui-property v-if="activeToggle" label="@ui.active" :is-text="true" class="is-toggle">
          <ui-toggle v-model="value.isActive" class="is-primary" :disabled="disabled" />
        </ui-property>
      </slot>
      <slot name="settings-properties"></slot>
    </div>
    <div class="ui-box is-light" v-if="value.id">
      <slot name="infos">
        <ui-property v-if="value.id && value.lastModifiedDate" label="@ui.modifiedDate" :is-text="true">
          <ui-date v-model="value.lastModifiedDate" />
        </ui-property>
        <ui-property v-if="value.id" label="@ui.createdDate" :is-text="true">
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

    inject: [ 'meta', 'disabled' ],

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
    }
  }
</script>
<template>
  <div>
    <ui-property v-if="value.id && value.lastModifiedDate"  field="lastModifiedDate" label="@ui.modifiedDate" :is-text="true" :vertical="true">
      <ui-date v-model="value.lastModifiedDate" />
    </ui-property>
    <ui-property v-if="value.id" label="@ui.createdDate" field="createdDate" :is-text="true" :vertical="true">
      <ui-date v-model="value.createdDate" />
    </ui-property>
  </div>
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

<style lang="scss">
  .editor-aside
  {
    padding-top: var(--padding);
    padding-left: var(--padding-s);
  }

  .editor-aside .ui-property + .ui-property
  {
    margin-top: var(--padding-m);
  }
</style>
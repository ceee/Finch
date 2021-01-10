<template>
  <div class="editor-infos">
    <div class="ui-box is-light editor-infos-aside">
      <slot name="before"></slot>
      <ui-property v-if="value.id && value.lastModifiedDate" field="lastModifiedDate" label="@ui.modifiedDate">
        <ui-date v-model="value.lastModifiedDate" />
      </ui-property>
      <ui-property v-if="value.id" label="@ui.createdDate" field="createdDate">
        <ui-date v-model="value.createdDate" />
      </ui-property>
      <slot name="after"></slot>
    </div>
  </div>
</template>


<script>
  export default {
    props: {
      value: {
        type: [Object, Array]
      },
      disabled: {
        type: Boolean,
        default: false
      }
    }
  }
</script>
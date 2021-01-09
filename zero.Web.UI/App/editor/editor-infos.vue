<template>
  <div class="editor-infos">
    <div class="ui-box is-light editor-infos-aside">
      <ui-property v-if="value.id && value.lastModifiedDate" field="lastModifiedDate" label="@ui.modifiedDate" :is-text="true" :vertical="true">
        <ui-date v-model="value.lastModifiedDate" />
      </ui-property>
      <ui-property v-if="value.id" label="@ui.createdDate" field="createdDate" :is-text="true" :vertical="true">
        <ui-date v-model="value.createdDate" />
      </ui-property>
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
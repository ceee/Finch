<template>
  <div v-if="visible" class="editor-infos">
    <div class="ui-box is-light editor-infos-aside">
      <slot name="before"></slot>
      <template v-if="value && value.id">
        <ui-property v-if="value.lastModifiedDate" field="lastModifiedDate" label="@ui.modifiedDate">
          <ui-date v-model="value.lastModifiedDate" />
        </ui-property>
        <ui-property label="@ui.createdDate" field="createdDate">
          <ui-date v-model="value.createdDate" />
        </ui-property>
         <ui-property label="@ui.entityfields.alias" field="alias">
          {{value.alias}}
        </ui-property>
        <ui-property label="@ui.entityfields.sort" field="sort">
          {{value.sort}}
        </ui-property>
        <ui-property v-if="value.key" label="@ui.entityfields.key" field="key">
          {{value.key}}
        </ui-property>
      </template>
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
    },

    computed: {
      visible()
      {
        return (this.value && this.value.id) || this.$scopedSlots.hasOwnProperty('before') || this.$scopedSlots.hasOwnProperty('after');
      }
    }
  }
</script>
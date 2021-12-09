<template>
  <div v-if="visible" class="editor-infos">
    <div class="ui-box is-light editor-infos-aside">
      <slot name="before"></slot>
      <template v-if="modelValue && modelValue.id">
        <ui-property v-if="modelValue.lastModifiedDate" field="lastModifiedDate" label="@ui.modifiedDate">
          <ui-date v-model="modelValue.lastModifiedDate" />
        </ui-property>
        <ui-property label="@ui.createdDate" field="createdDate">
          <ui-date v-model="modelValue.createdDate" />
        </ui-property>
        <ui-property label="@ui.entityfields.alias" field="alias">
          {{modelValue.alias}}
        </ui-property>
        <ui-property label="@ui.entityfields.sort" field="sort">
          {{modelValue.sort}}
        </ui-property>
        <ui-property v-if="modelValue.key" label="@ui.entityfields.key" field="key">
          {{modelValue.key}}
        </ui-property>
      </template>
      <slot name="after"></slot>
    </div>
  </div>
</template>


<script>
  export default {
    props: {
      modelValue: {
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
        return (this.modelValue && this.modelValue.id) || this.$slots.hasOwnProperty('before') || this.$slots.hasOwnProperty('after');
      }
    }
  }
</script>
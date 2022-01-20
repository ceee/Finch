<template>
  <ui-editor-page :api="api" editor="translations:edit" title="@translation.name" prefix="@translation.list" route="translations-edit">
    <template v-slot:header="slotProps">
      <ui-form-header v-model:value="slotProps.model" prefix="@translation.list" :title="slotProps.model.key || '@translation.name'" :force-title="true" :title-disabled="true" 
                      :disabled="slotProps.readonly" :is-create="!slotProps.id" :state="slotProps.form.state" :can-delete="slotProps.canDelete" @delete="slotProps.onInternalDelete" />
    </template>
  </ui-editor-page>
</template>

<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
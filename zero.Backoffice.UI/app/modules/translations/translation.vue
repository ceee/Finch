<template>
  <ui-editor-page :api="api" editor="translations:edit" title="@translation.name" prefix="@translation.list" route="translations-edit" />
</template>

<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
<template>
  <ui-editor-page :api="api" editor="languages:edit" title="@language.name" prefix="@language.list" route="languages-edit" :disabled="true" />
</template>


<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
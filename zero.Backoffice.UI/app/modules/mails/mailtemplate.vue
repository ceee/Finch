<template>
  <ui-editor-page :api="api" editor="mailtemplates:edit" title="@mailtemplate.name" prefix="@mailtemplate.list" route="mailtemplates-edit" />
</template>

<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
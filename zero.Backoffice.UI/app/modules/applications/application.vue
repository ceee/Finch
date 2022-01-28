<template>
  <ui-editor-page :api="api" editor="applications:edit" title="@application.name" prefix="@application.list" route="applications-edit" :can-delete="false" />
</template>


<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
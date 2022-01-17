<template>
  <ui-editor-page :api="api" editor="users:edit" title="@user.name" prefix="@user.users" route="users-edit" />
</template>


<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
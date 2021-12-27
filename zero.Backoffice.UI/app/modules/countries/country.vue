<template>
  <ui-editor-page :api="api" editor="countries:edit" title="@country.name" prefix="@country.list" route="countries-edit" />
</template>


<script>
  import api from './api';
  export default {
    data: () => ({ api })
  }
</script>
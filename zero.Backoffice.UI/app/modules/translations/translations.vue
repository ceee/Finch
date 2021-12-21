<template>
  <div class="translations">
    <ui-header-bar title="@translation.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" alias="translations" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="translations" @count="count = $event" />
    </div>
  </div>
</template>


<script lang="ts">
  export default {
    data: () => ({
      count: 0,
      createRoute: 'translations-edit'
    })
  
  }
</script>
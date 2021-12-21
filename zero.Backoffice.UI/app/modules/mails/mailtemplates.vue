<template>
  <div class="mailtemplates">
    <ui-header-bar title="@mailtemplate.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" alias="countries" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="mailtemplates" @count="count = $event" />
    </div>
  </div>
</template>


<script lang="ts">
  export default {
    data: () => ({
      count: 0,
      createRoute: 'mailtemplates-edit'
    })
  }
</script>
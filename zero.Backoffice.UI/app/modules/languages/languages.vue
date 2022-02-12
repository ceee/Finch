<template>
  <div class="languages">
    <ui-header-bar title="@language.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" alias="languages" v-if="false" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="languages" @count="count = $event" />
    </div>
  </div>
</template>


<script lang="ts">
  export default {
    data: () => ({
      count: 0,
      createRoute: 'languages-edit'
    })
  
  }
</script>
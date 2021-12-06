<template>
  <div class="languages">
    <ui-header-bar title="@language.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" blueprint-alias="language" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="languages" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.settings.languages + '-create'
    })
  }
</script>
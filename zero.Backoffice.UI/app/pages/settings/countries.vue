<template>
  <div class="countries">
    <ui-header-bar title="@country.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" blueprint-alias="country" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="countries" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.settings.countries + '-create'
    })
  }
</script>
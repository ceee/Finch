<template>
  <div class="languages">
    <ui-header-bar title="@mailTemplate.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="mailTemplates" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.settings.mails + '-create'
    })
  }
</script>
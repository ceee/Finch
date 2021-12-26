<template>
  <div class="users">
    <ui-header-bar title="@user.users" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" alias="countries" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="users" @count="count = $event" />
    </div>
  </div>
</template>


<script lang="ts">
  export default {
    data: () => ({
      count: 0,
      createRoute: 'users-edit'
    })
  }
</script>

<style lang="scss">
  .users .ui-table-field-image
  {
    border-radius: 50px;
  }

  .users .ui-table-cell[table-field="avatarId"]
  {
    padding: 12px;
    padding-left: 20px;
  }

  .users .ui-table-cell[table-field="name"]
  {
    border-left-color: transparent;
  }
</style>
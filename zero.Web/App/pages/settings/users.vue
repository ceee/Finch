<template>
  <div class="users">
    <ui-header-bar title="Users & Permissions">
      <ui-button type="light" label="Add user" icon="fth-plus" />
      <ui-button type="light" label="Add role" icon="fth-plus" />
      <!--<ui-table-filter :filter="false" />-->
    </ui-header-bar>
    <div class="ui-blank-box">
      <!--<ui-table :config="tableConfig" />-->
    </div>
  </div>
</template>


<script>
  import AuthApi from 'zero/services/auth.js'

  export default {
    data: () => ({
      _user: null
    }),

    created()
    {
      this._user = AuthApi.user;
    },

    methods: {
      onBack()
      {
        this.$router.go(-1);
      }
    }
  }
</script>


<style lang="scss">
  
</style>
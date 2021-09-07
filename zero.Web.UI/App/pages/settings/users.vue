<template>
  <div class="users">
    <ui-header-bar title="@user.title" :back-button="true">
      <ui-table-filter :attach="$refs.usersTable" />
      <ui-add-button type="light onbg" :route="createRoleRoute" label="@user.add_role" />
      <ui-button type="primary" label="@user.add_user" />
    </ui-header-bar>

    <div class="ui-blank-box">
      <h2 class="ui-headline users-group-headline" v-localize="'@role.roles'"></h2>
      <div class="users-roles">
        <router-link v-for="role in roles" :key="role.id" :to="getRoleLink(role)" class="users-role">
          <ui-icon class="users-role-icon" :symbol="role.icon" :size="24" />
          <strong>{{role.name}}</strong>
          <span class="users-role-minor" v-localize="{ key: role.countClaims !== 1 ? '@user.count_permissions' : '@user.one_permission', tokens: { count: role.claims.length }}"></span>
        </router-link>
      </div>
    </div>

    <div class="ui-blank-box">
      <h2 class="ui-headline users-group-headline" v-localize="'@user.users'"></h2>
      <ui-table ref="usersTable" config="users" />
    </div>
  </div>
</template>


<script>
  import UserRolesApi from 'zero/api/userRoles.js';

  export default {
    data: () => ({
      createRoleRoute: 'roles-create',
      roles: []
    }),

    created()
    {
      UserRolesApi.getAll().then(items =>
      {
        this.roles = items;
      });
    },

    methods: {

      getRoleLink(item)
      {
        return {
          name: 'roles-edit',
          params: { id: item.id },
          query: { scope: 'shared '}
        };
      }
    }
  }
</script>


<style lang="scss">
  h2.users-group-headline
  {
    margin-bottom: 30px !important;
  }

  .users .ui-table-field-image
  {
    border-radius: 50px;
  }

  .users .ui-table-cell[table-field="avatar"]
  {
    padding: 12px;
    padding-left: 20px;
  }

  .users .ui-table-cell[table-field="name"]
  {
    border-left-color: transparent;
  }

  .users-roles
  {
    display: grid;
    gap: var(--padding);
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    align-items: stretch;
    margin-bottom: calc(var(--padding) * 2);
  }

  a.users-role
  {
    display: flex;
    flex-direction: column;
    background: var(--color-box);
    border-radius: var(--radius);
    padding: var(--padding-m) var(--padding);
    text-align: center;
    color: var(--color-text);
    font-size: var(--font-size);
    line-height: 1.5;
    box-shadow: var(--shadow-short);
  }

  .users-role-minor
  {
    color: var(--color-text-dim);
  }

  .users-role-icon
  {
    text-align: center;
    display: inline-block;
    margin: 0 auto var(--padding-m);
    position: relative;
  }
</style>
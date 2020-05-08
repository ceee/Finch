<template>
  <div class="users">
    <ui-header-bar title="Users & Permissions" :back-button="true">
      <ui-table-filter v-model="usersConfig" />
      <ui-button type="white" label="Add role" icon="fth-plus" />
      <ui-button label="Add user" icon="fth-plus" />
    </ui-header-bar>

    <div class="ui-blank-box">
      <h2 class="ui-headline users-group-headline" v-localize="'@role.roles'"></h2>
      <div class="users-roles">
        <router-link v-for="role in roles" :key="role.id" :to="getRoleLink(role)" class="users-role">
          <i class="users-role-icon" :class="role.icon"></i>
          <strong>{{role.name}}</strong>
          <span class="users-role-minor" v-localize="{ key: role.countClaims !== 1 ? '@user.count_permissions' : '@user.one_permission', tokens: { count: role.countClaims }}"></span>
        </router-link>
      </div>
    </div>

    <div class="ui-blank-box">
      <h2 class="ui-headline users-group-headline" v-localize="'@user.users'"></h2>
      <ui-table v-model="usersConfig" />
    </div>
  </div>
</template>


<script>
  import AuthApi from 'zero/services/auth.js'
  import UserRolesApi from 'zero/resources/userRoles.js';
  import UsersApi from 'zero/resources/users.js';
  import { filter as _filter, map as _map } from 'underscore';

  export default {
    data: () => ({
      roles: [],
      usersConfig: {}
    }),

    created()
    {
      UserRolesApi.getAll().then(items =>
      {
        this.roles = items;
      });

      this.usersConfig = {
        labelPrefix: '@user.fields.',
        search: null,
        columns: {
          avatar: {
            label: '',
            as: 'html',
            render: item => `<img src=${item.avatar} class="users-list-avatar">`,
            width: 70,
            link: this.getUserLink
          },
          name: {
            label: '@ui.name',
            as: 'text',
            bold: true,
            link: this.getUserLink
          },
          email: 'text',
          roles: {
            as: 'text',
            render: item =>
            {
              const roles = _filter(this.roles, role => item.roles.indexOf(role.alias) > -1);
              return _map(roles, role => role.name).join(', ');
            }
          },
          isActive: {
            as: 'bool',
            width: 200
          }
        },
        items: UsersApi.getAll
      };
    },

    methods: {

      getRoleLink(item)
      {
        return {
          name: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-role',
          params: { id: item.id }
        };
      },

      getUserLink(item)
      {
        return {
          name: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-edit',
          params: { id: item.id }
        };
      },
    }
  }
</script>


<style lang="scss">
  h2.users-group-headline
  {
    margin-bottom: 30px;
  }

  .users-list-avatar
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
    grid-gap: var(--padding);
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
    padding: var(--padding-s) var(--padding);
    text-align: center;
    color: var(--color-fg);
    font-size: var(--font-size);
    line-height: 1.5;
    transition: box-shadow 0.2s ease;
    box-shadow: var(--color-shadow-short);

    &:hover
    {
      box-shadow: 0 0 20px var(--color-shadow);
    }
  }

  .users-role-minor
  {
    color: var(--color-fg-mid);
  }

  .users-role-icon
  {
    font-size: 26px;
    text-align: center;
    display: inline-block;
    margin: 0 auto var(--padding-s);
    position: relative;
  }
</style>
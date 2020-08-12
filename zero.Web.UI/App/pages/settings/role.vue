<template>
  <ui-form ref="form" class="role" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@role.name" :disabled="disabled" :is-create="!id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete" />
    <ui-editor config="userRole" v-model="model" :meta="meta" :active-toggle="false" />
  </ui-form>
</template>


<script>
  import { filter as _filter } from 'underscore';
  import UserRolesApi from 'zero/resources/userRoles';
  import UiEditor from 'zero/editor/editor';

  export default {
    props: ['id'],

    components: { UiEditor },

    data: () => ({
      meta: {},
      model: { name: null, features: [], domains: [], claims: [] },
      route: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-role',
      disabled: false
    }),

    methods: {

      onLoad(form)
      {
        form.load(!this.id ? UserRolesApi.getEmpty() : UserRolesApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
        });
      },


      onSubmit(form)
      {
        form.handle(UserRolesApi.save(this.model));
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(UserRolesApi.delete.bind(this, this.id));
      }     
    }
  }
</script>

<style lang="scss">
  .role-permission-toggle + .role-permission-toggle
  {
    padding-top: 0;
    margin-top: var(--padding);
  }

  .role .ui-box + .ui-permissions
  {
    margin-top: var(--padding);
  }
</style>
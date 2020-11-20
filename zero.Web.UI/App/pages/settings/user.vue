<template>
  <ui-form ref="form" class="user" v-slot="form" @submit="onSubmit" @load="onLoad" :route="route">
    <ui-form-header v-model="model" title="@user.name" :disabled="disabled" :is-create="!$route.params.id" :state="form.state" :can-delete="meta.canDelete" @delete="onDelete">
      <template v-slot:actions>
        <ui-dropdown-button v-if="model.isActive" label="Disable" icon="fth-minus-circle" @click="onActiveChange" :disabled="disabled" />
        <ui-dropdown-button v-if="!model.isActive" label="Enable" icon="fth-plus-circle" @click="onActiveChange" :disabled="disabled" />
        <ui-dropdown-button label="Change password" icon="fth-lock" :disabled="disabled" />
      </template>
    </ui-form-header>
    <ui-editor config="user" v-model="model" :meta="meta" :disabled="disabled">
      <template v-slot:settings>
        <ui-property label="@user.fields.isLockedOut" :is-text="true" class="is-toggle">
          <ui-toggle :value="isLockedOut" :negative="true" @input="onLockoutChange" :disabled="disabled" />
        </ui-property>
        <p v-if="isLockedOut" class="ui-message type-error block user-aside-error">
          <i class="ui-message-icon fth-alert-circle"></i>
          <span class="ui-message-text">
            <span v-localize:html="'@user.fields.isLockedOut_warning'"></span>:<br>
            <strong><ui-date v-model="model.lockoutEnd" format="long" /></strong>
          </span>
        </p>
        <ui-message v-if="!model.isActive" class="user-aside-error" type="error" text="@user.fields.isDisabled_warning" />
      </template>
    </ui-editor>
  </ui-form>
</template>

<script>
  import { filter as _filter } from 'underscore';
  import UsersApi from 'zero/api/users.js';
  import UiEditor from 'zero/editor/editor.vue';

  export default {
    components: { UiEditor },

    data: () => ({
      meta: {},
      model: {
        avatarId: null,
        name: null,
        email: null,
        lockoutEnd: null
      },
      route: zero.alias.settings.users + '-edit',
      disabled: false,
      originalLockoutEnd: null
    }),

    computed: {
      isLockedOut()
      {
        return !!this.model.lockoutEnd;
      }
    },

    methods: {

      onLoad(form)
      {
        form.load(!this.$route.params.id ? UsersApi.getEmpty() : UsersApi.getById(this.$route.params.id)).then(response =>
        {
          this.disabled = !response.meta.canEdit;
          this.meta = response.meta;
          this.model = response.entity;
          this.originalLockoutEnd = this.model.lockoutEnd;
        });
      },


      onSubmit(form)
      {
        form.handle(UsersApi.save(this.model)).then(res =>
        {
          if (res.entity.id === AuthApi.user.id)
          { 
            AuthApi.setUser(res.entity);
          }
        });
      },


      onDelete(item, opts)
      {
        opts.hide();
        this.$refs.form.onDelete(UsersApi.delete.bind(this, this.$route.params.id));
      },


      onLockoutChange(locked)
      {
        this.model.lockoutEnd = locked ? this.originalLockoutEnd : null;
      },


      onActiveChange(value, opts)
      {
        opts.loading(true);

        const isActive = !this.model.isActive;
        let promise = isActive ? UsersApi.enable(this.model) : UsersApi.disable(this.model);

        promise.then(response =>
        {
          opts.loading(false);
          opts.hide();

          if (response.success)
          {
            this.model.isActive = isActive;
          }
        });
      },
    }
  }
</script>

<style lang="scss">
  .user-aside-error
  {
    margin-bottom: 0;
  }
</style>
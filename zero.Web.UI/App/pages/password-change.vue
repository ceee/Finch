<template>
  <ui-form v-if="!loading" ref="form" class="app-password-change" v-slot="form" @submit="onSubmit">
    <h2 class="ui-headline" v-localize="config.title"></h2>

    <div class="app-password-change-fields">
      <ui-property label="@changepasswordoverlay.fields.currentPassword" :required="true" :vertical="true">
        <input v-model="model.currentPassword" type="password" class="ui-input" maxlength="160" />
      </ui-property>
      <ui-property label="@changepasswordoverlay.fields.newPassword" :required="true" :vertical="true">
        <input v-model="model.newPassword" type="password" class="ui-input" maxlength="160" />
      </ui-property>
      <ui-property label="@changepasswordoverlay.fields.confirmNewPassword" :required="true" :vertical="true">
        <input v-model="model.confirmNewPassword" type="password" class="ui-input" maxlength="160" />
        <ui-error :catch-remaining="true" />
      </ui-property>
    </div>

    <div class="app-confirm-buttons">
      <ui-button type="action" :submit="true" :state="form.state" :label="config.confirmLabel"></ui-button>
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  import AuthApi from 'zero/services/auth.js'
  import UsersApi from 'zero/resources/users.js'

  export default {

    props: {
      config: Object
    },

    data: () => ({
      loading: false,
      model: {
        currentPassword: null,
        newPassword: null,
        confirmNewPassword: null
      }
    }),

    methods: {

      onSubmit(form)
      {
        form.handle(UsersApi.updatePassword(this.model)).then(res =>
        {
          setTimeout(() =>
          {
            AuthApi.rejectUser("@login.rejectReasons.passwordchanged");
          }, 500);
        }, errors =>
        {
          console.info('login: error', errors);
        });
      }
    }
  }
</script>

<style lang="scss">
  .app-password-change
  {
    text-align: left;
  }

  .app-password-change-fields
  {
    margin-top: var(--padding);

    .ui-property + .ui-property, .ui-split + .ui-property
    {
      margin-top: 0;
    }
  }

  /*.translation-items
  {
    margin-top: var(--padding);

    .ui-property + .ui-property,
    .ui-split + .ui-property
    {
      margin-top: 0;
    }
  }*/
</style>
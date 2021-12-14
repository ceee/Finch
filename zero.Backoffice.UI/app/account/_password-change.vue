<template>
  <ui-form v-if="!loading" ref="form" class="app-password-change" v-slot="form" @submit="onSubmit">
    <h2 class="ui-headline" v-localize="'@changepasswordoverlay.title'"></h2>

    <div class="app-password-change-fields">
      <ui-error :catch-remaining="true" />
      <ui-editor :config="editorConfig" v-model="model" />
    </div>

    <div class="app-confirm-buttons">
      <ui-button type="accent" :submit="true" :state="form.state" label="@ui.confirm"></ui-button>
      <ui-button type="light" label="@ui.close" :disabled="loading" @click="config.close"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  import UsersApi from 'zero/api/users.js'
  import passwordRenderer from 'zero/renderers/editors/password.js';

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
      },
      editorConfig: passwordRenderer
    }),

    methods: {

      onSubmit(form)
      {
        form.handle(UsersApi.hashPassword({ ...this.model, userId: this.$route.params.id })).then(res =>
        {
          this.config.confirm(res.model);
        }, errors => {});
      }
    }
  }
</script>

<style lang="scss">
  .app-password-change
  {
    text-align: left;

    .editor, .ui-tab, .ui-box
    {
      padding: 0;
    }
  }

  .app-password-change-fields
  {
    margin-top: var(--padding);

    .ui-property + .ui-property, .ui-split + .ui-property
    {
      margin-top: var(--padding);
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
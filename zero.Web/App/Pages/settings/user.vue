<template>
  <ui-form ref="form" class="user" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@user.name">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="white" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button :submit="true" label="Save" :state="form.state" />
    </ui-header-bar>

    <ui-tabs>

      <ui-tab class="ui-view-box has-sidebar" label="General">

        <div class="ui-box">
          <ui-property label="@ui.name" :required="true">
            <input v-model="model.name" type="text" class="ui-input" v-localize:placeholder="'@user.fields.name_placeholder'" />
          </ui-property>
          <ui-property label="@user.fields.email" description="@user.fields.email_text" :required="true">
            <input v-model.trim="model.email" type="email" class="ui-input" v-localize:placeholder="'@user.fields.email_placeholder'" />
          </ui-property>
          <ui-property label="@user.fields.avatar" description="@user.fields.avatar_text" :required="true">
            <ui-media :config="avatarConfig" v-model="model.avatar" :disabled="disabled" />
          </ui-property>
          <ui-property label="@user.fields.language" description="@user.fields.language_text" :required="true">
            <div class="ui-native-select">
              <select v-model="model.languageId">
                <option v-for="culture in model.supportedCultures" :value="culture.code">{{culture.name}}</option>
              </select>
            </div>
          </ui-property>
        </div>

        <aside class="user-aside">
          <div class="ui-view-box-aside ui-box">

            <ui-property label="@user.fields.isLockedOut" :vertical="true" :is-text="true">
              <ui-toggle :value="isLockedOut" :negative="true" @input="onLockoutChange" />
              <p v-if="isLockedOut" class="ui-message type-error block user-aside-error">
                <i class="ui-message-icon fth-alert-circle"></i>
                <span class="ui-message-text">
                  <span v-localize:html="'@user.fields.isLockedOut_warning'"></span>:<br>
                  <strong><ui-date v-model="model.lockoutEnd" format="long" /></strong>
                </span>
              </p>
              <ui-message v-if="!model.isActive" class="user-aside-error" type="error" text="@user.fields.isDisabled_warning" />
            </ui-property>
          </div>
          <div class="ui-view-box-aside">
            <ui-property label="@ui.id" :vertical="true" :is-text="true">
              {{model.id}}
            </ui-property>
            <ui-property label="@ui.createdDate" :vertical="true" :is-text="true">
              <ui-date v-model="model.createdDate" />
            </ui-property>
          </div>
        </aside>
      </ui-tab>

      <ui-tab label="Permissions">
        <div class="ui-box">
          <ui-property label="@user.fields.roles" description="@user.fields.roles_text" :required="true">
            select
          </ui-property>
        </div>
        <div class="ui-box">
          <ui-property label="@user.fields.sections" description="@user.fields.sections_text" :required="true">
            select
          </ui-property>
        </div>
      </ui-tab>

    </ui-tabs>
  </ui-form>
</template>


<script>
  import UsersApi from 'zero/resources/users';
  import Strings from 'zero/services/strings';

  export default {
    props: ['id'],

    name: 'app-settings-user',

    data: () => ({
      loading: true,
      disabled: false,
      actions: [],
      model: {
        name: null,
        email: null,
        supportedCultures: []
      },
      avatarConfig: {
        display: 'big',
        fileExtensions: zero.config.media.defaults.images
      },
      originalLockoutEnd: null
    }),

    created()
    {
      this.actions.push({
        key: 'disabled',
        name: 'Disable',
        icon: 'fth-minus-circle',
        action: (item, opts) =>
        {
          this.onActiveChange(opts);
        }
      });
      this.actions.push({
        name: 'Change password',
        icon: 'fth-lock'
      });
      this.actions.push({
        name: 'Delete',
        icon: 'fth-x'
      });
    },


    computed: {
      isLockedOut()
      {
        return !!this.model.lockoutEnd;
      }
    },


    beforeRouteLeave(to, from, next) 
    {
      this.$refs.form.beforeRouteLeave(to, from, next);
    },


    methods: {

      onBack()
      {
        this.$router.go(-1);
      },

      onLoad(form)
      {
        form.load(UsersApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.canEdit;
          this.model = response;
          this.originalLockoutEnd = this.model.lockoutEnd;

          if (!this.model.isActive)
          {
            this.actions[0].name = 'Enable';
            this.actions[0].icon = 'fth-plus-circle';
          }
        });
      },

      onSubmit(form)
      {
        form.handle(new Promise(resolve =>
        {
          setTimeout(() =>
          {
            resolve(true);
          }, 1000);
        })).then(() =>
        {
          form.setDirty(false);
        });
      },

      onLockoutChange(locked)
      {
        this.model.lockoutEnd = locked ? this.originalLockoutEnd : null;
      },

      onActiveChange(opts)
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
            this.actions[0].name = isActive ? 'Disable' : 'Enable';
            this.actions[0].icon = isActive ? 'fth-minus-circle' : 'fth-plus-circle';
          }
        });
      },

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      }
    }
  }
</script>

<style lang="scss">
  .user-aside
  {
    width: 360px;
  }
  .user-aside-error
  {
    margin-bottom: 0;
  }
</style>
<template>
  <ui-form ref="form" class="user" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@user.name" :on-back="onBack">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="light" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list :items="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button :submit="true" label="Save" :state="form.state" />
    </ui-header-bar>

    <ui-tabs>

      <ui-tab class="ui-box" label="General">
        <ui-property label="@user.fields.name" :required="true">
          <input v-model="model.name" type="text" class="ui-input" v-localize:placeholder="'@user.fields.name_placeholder'" />
        </ui-property>
        <ui-property label="@user.fields.email" description="@user.fields.email_text" :required="true">
          <input v-model.trim="model.email" type="email" class="ui-input" v-localize:placeholder="'@user.fields.email_placeholder'" />
        </ui-property>
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

  export default {
    props: ['id'],

    name: 'app-settings-user',

    data: () => ({
      loading: true,
      page: true,
      actions: [],
      model: {
        name: null,
        email: null
      }
    }),

    created()
    {
      this.actions.push({
        name: 'Disable',
        icon: 'fth-minus-circle'
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
          console.info(response);
          this.model = response;
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

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      }
    }
  }
</script>


<style lang="scss">
  
</style>
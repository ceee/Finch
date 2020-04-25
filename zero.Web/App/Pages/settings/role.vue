<template>
  <ui-form ref="form" class="role" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@role.name" :back-button="true">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="light" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>
      <ui-button :submit="true" label="@ui.save" :state="form.state" />
    </ui-header-bar>

    <div class="ui-view-box has-sidebar" label="@ui.tab_general">
      <div>

        <div class="ui-box">
          <ui-property label="@ui.name" :required="true">
            <input v-model="model.name" type="text" class="ui-input" maxlength="120" />
          </ui-property>
          <ui-property label="@role.fields.description">
            <input v-model="model.description" type="text" class="ui-input" maxlength="200" />
          </ui-property>
          <ui-property label="@role.fields.icon" :required="true">
            <ui-iconpicker v-model="model.icon" />
          </ui-property>
        </div>

        <div class="ui-box">
          <h2 class="ui-headline" v-localize="'@permission.sections'"></h2>
          <ui-property class="role-permission-toggle" v-for="section in permissions.sections" :label="section.name">
            <ui-toggle v-model="section.toggled" />
          </ui-property>
        </div>

        <div class="ui-box">
          <h2 class="ui-headline" v-localize="'@permission.settings'"></h2>
          <ui-property class="role-permission-toggle" v-for="setting in permissions.settings" :label="setting.name">
            <ui-toggle v-model="setting.toggled" />
          </ui-property>
        </div>
      </div>

      <aside class="ui-view-box-aside">
        <ui-property label="@ui.id" :vertical="true" :is-text="true">
          {{model.id}}
        </ui-property>
        <ui-property label="@ui.createdDate" :vertical="true" :is-text="true">
          <ui-date v-model="model.createdDate" />
        </ui-property>
      </aside>
    </div>

  </ui-form>
</template>


<script>
  import UserRolesApi from 'zero/resources/userRoles';
  import Overlay from 'zero/services/overlay.js'

  export default {
    props: ['id'],

    data: () => ({
      loading: true,
      page: true,
      actions: [],
      model: {
        name: null,
        email: null
      },
      permissions: {
        sections: [],
        settings: []
      }
    }),

    created()
    {
      this.actions.push({
        name: 'Delete',
        icon: 'fth-trash',
        action: this.onDelete
      });

      this.permissions.sections = zero.sections;

      zero.settingsAreas.forEach(area =>
      {
        area.items.forEach(item =>
        {
          this.permissions.settings.push(item);
        });
      })
    },


    beforeRouteLeave(to, from, next) 
    {
      this.$refs.form.beforeRouteLeave(to, from, next);
    },


    methods: {

      onLoad(form)
      {
        form.load(UserRolesApi.getById(this.id)).then(response =>
        {
          this.model = response;
        });
      },

      onSubmit(form)
      {
        //this.model.id = "zero.countries.16-B";
        form.handle(UserRolesApi.save(this.model)).then(response =>
        {
          console.info(response);
        });
      },

      onDelete(item, opts)
      {
        opts.hide();

        Overlay.confirmDelete().then((opts) =>
        {
          console.info('click');
          opts.state('loading');

          UserRolesApi.delete(this.id).then(response =>
          {
            if (response.success)
            {
              opts.state('success');
              opts.hide();
              this.$router.go(-1);
              // TODO show message
            }
            else
            {
              opts.errors(response.errors);
            }
          });
        }); 
      }
    }
  }
</script>

<style lang="scss">
  .role-permission-toggle + .role-permission-toggle
  {
    padding-top: 0;
  }

  .role .-m-top
  {
    margin-top: var(--padding);
  }
</style>
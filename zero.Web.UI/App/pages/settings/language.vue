<template>
  <ui-form ref="form" class="language" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@language.name" :back-button="true">
      <ui-dropdown align="right" v-if="!disabled">
        <template v-slot:button>
          <ui-button type="white" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>
      <ui-button :submit="true" label="@ui.save" :state="form.state" v-if="!disabled" />
    </ui-header-bar>

    <div class="ui-view-box has-sidebar">
      <div>
        <div class="ui-box">
          <ui-property label="@ui.name" :required="true">
            <input v-model="model.name" type="text" class="ui-input" maxlength="80" :readonly="disabled" />
          </ui-property>
          <ui-property label="@language.fields.locale" :required="true">
            <div class="ui-native-select">
              <select v-model="model.code">
                <option v-for="culture in cultures" :value="culture.code">{{culture.name}}</option>
              </select>
            </div>
          </ui-property>
          <ui-property label="@language.fields.isDefault" description="@language.fields.isDefault_text">
            <ui-toggle v-model="model.isDefault" />
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
  import LanguagesApi from 'zero/resources/languages';
  import Overlay from 'zero/services/overlay.js';

  export default {
    props: ['id'],

    data: () => ({
      actions: [],
      model: { name: null },
      disabled: false,
      cultures: []
    }),

    created()
    {
      this.actions.push({
        name: 'Delete',
        icon: 'fth-trash',
        action: this.onDelete
      });
    },


    beforeRouteLeave(to, from, next) 
    {
      this.$refs.form.beforeRouteLeave(to, from, next);
    },


    methods: {

      onLoad(form)
      {
        form.load(LanguagesApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.canEdit;
          this.model = response.entity;
        });

        LanguagesApi.getAllCultures().then(res =>
        {
          this.cultures = res;
        });
      },


      onSubmit(form)
      {
        form.handle(LanguagesApi.save(this.model)).then(response =>
        {
          console.info(response);
        });
      },

      onDelete(item, opts)
      {
        opts.hide();

        Overlay.confirmDelete().then((opts) =>
        {
          opts.state('loading');

          LanguagesApi.delete(this.id).then(response =>
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
  /*.role-permission-toggle + .role-permission-toggle
  {
    padding-top: 0;
  }

  .role .ui-box + .ui-permissions
  {
    margin-top: var(--padding);
  }*/
</style>
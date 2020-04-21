<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@country.name">
      <ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="light" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list :items="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button :submit="true" label="Save" :state="form.state" />
    </ui-header-bar>

    <ui-tabs>

      <ui-tab class="ui-form-box has-sidebar" label="General">
        <div class="ui-box">
          <ui-property label="@ui.name" :required="true">
            <input v-model="model.name" type="text" class="ui-input" />
          </ui-property>
          <ui-property label="@country.fields.code" description="@country.fields.code_text" :required="true">
            <input v-model="model.code" type="text" class="ui-input country-flag-input" maxlength="2" />
          </ui-property>
          <ui-property label="@country.fields.isPreferred">
            <ui-toggle v-model="model.isPreferred" />
          </ui-property>
        </div>
        <aside class="ui-form-box-aside">
          <ui-property label="@ui.active" :vertical="true">
            <ui-toggle v-model="model.isActive" />
          </ui-property>
          <ui-property label="@ui.id" :vertical="true" :is-text="true">
            {{model.id}}
          </ui-property>
          <ui-property label="@ui.createdDate" :vertical="true" :is-text="true">
            <ui-date v-model="model.createdDate" />
          </ui-property>
        </aside>
      </ui-tab>

    </ui-tabs>
  </ui-form>
</template>


<script>
  import CountriesApi from 'zero/resources/countries';

  export default {
    props: ['id'],

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
        form.load(CountriesApi.getById(this.id)).then(response =>
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
  .country .country-flag-input
  {
    max-width: 80px;
  }
</style>
<template>
  <ui-form ref="form" class="country" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@country.name">
      <!--<ui-dropdown align="right">
        <template v-slot:button>
          <ui-button type="light" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>-->
      <ui-button :submit="true" label="@ui.save" :state="form.state" />
    </ui-header-bar>

    <div class="ui-view-box has-sidebar" label="@ui.tab_general">
      <div class="ui-box">
        <ui-property label="@ui.name" :required="true">
          <input v-model="model.name" type="text" class="ui-input" maxlength="120" />
        </ui-property>
        <ui-property label="@country.fields.code" description="@country.fields.code_text" :required="true">
          <input v-model="model.code" type="text" class="ui-input country-flag-input" maxlength="2" />
        </ui-property>
        <ui-property label="@country.fields.isPreferred">
          <ui-toggle v-model="model.isPreferred" />
        </ui-property>
      </div>
      <aside class="ui-view-box-aside">
        <ui-property label="@ui.active" :vertical="true" :is-text="true">
          <ui-toggle v-model="model.isActive" />
        </ui-property>
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
  import CountriesApi from 'zero/resources/countries';
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
      }
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
        form.load(CountriesApi.getById(this.id)).then(response =>
        {
          this.model = response;
        });
      },

      onSubmit(form)
      {
        //this.model.id = "zero.countries.16-B";
        form.handle(CountriesApi.save(this.model)).then(response =>
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

          CountriesApi.delete(this.id).then(response =>
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
  .country .country-flag-input
  {
    max-width: 80px;
  }
</style>
<template>
  <ui-form ref="form" class="application" v-slot="form" @submit="onSubmit" @load="onLoad">

    <ui-header-bar :title="model.name" title-empty="@application.name" :back-button="true">
      <ui-dropdown align="right" v-if="!disabled">
        <template v-slot:button>
          <ui-button type="light" label="@ui.actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" />
      </ui-dropdown>
      <ui-button :submit="true" label="@ui.save" :state="form.state" v-if="!disabled" />
    </ui-header-bar>

    <ui-tabs>

      <ui-tab label="@ui.tab_general" class="ui-view-box has-sidebar">
        <div>
          <div class="ui-box">
            <ui-property label="@ui.name" description="@application.fields.name_text" :required="true">
              <input v-model="model.name" type="text" class="ui-input" maxlength="80" :readonly="disabled" />
            </ui-property>
            <ui-property label="@application.fields.fullName" description="@application.fields.fullName_text" :required="true">
              <input v-model="model.fullName" type="text" class="ui-input" maxlength="150" :readonly="disabled" />
            </ui-property>
            <ui-property label="@application.fields.email" description="@application.fields.email_text" :required="true">
              <input v-model="model.email" type="email" class="ui-input" maxlength="200" :readonly="disabled" />
            </ui-property>
            <ui-property label="@application.fields.image" description="@application.fields.image_text" :required="true">
              <ui-media :config="mediaConfig" v-model="model.image" />
            </ui-property>
            <ui-property label="@application.fields.icon" description="@application.fields.icon_text" :required="true">
              <ui-media :config="mediaConfig" v-model="model.icon" />
            </ui-property>
          </div>
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
      </ui-tab>

      <ui-tab label="@application.tab_domains" class="ui-box">
        <ui-property label="@application.fields.domains" description="@application.fields.domains_text" :required="true">
          <ui-input-list v-model="model.domains" :disabled="disabled" add-label="@application.fields.domains_add" />
          <p class="ui-property-help" v-localize="'@application.fields.domains_help'"></p>
        </ui-property>
      </ui-tab>

      <ui-tab v-if="features.length > 0" label="@application.tab_features" class="ui-box" :count="model.features.length">
        <ui-property v-for="feature in features" :label="feature.name" :description="feature.description">
          <ui-toggle :value="model.features.indexOf(feature.alias) > -1" @input="onFeatureToggle($event, feature)" />
        </ui-property>
      </ui-tab>

    </ui-tabs>
  </ui-form>
</template>


<script>
  import ApplicationsApi from 'zero/resources/applications';
  import Overlay from 'zero/services/overlay.js';

  export default {
    props: ['id'],

    data: () => ({
      actions: [],
      model: { name: null, features: [] },
      disabled: false,
      features: [],
      mediaConfig: {
        display: 'big',
        fileExtensions: zero.config.media.defaults.images_artificial
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
        form.load(ApplicationsApi.getById(this.id)).then(response =>
        {
          this.disabled = !response.canEdit;
          this.model = response;
        });

        ApplicationsApi.getAllFeatures().then(items =>
        {
          this.features = items;
        });
      },


      onSubmit(form)
      {
        form.handle(ApplicationsApi.save(this.model)).then(response =>
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

          ApplicationsApi.delete(this.id).then(response =>
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
      },


      onFeatureToggle(isOn, feature)
      {
        const alias = feature.alias;
        const index = this.model.features.indexOf(alias);

        if (!isOn && index > -1)
        {
          this.model.features.splice(index, 1);
        }
        else if (isOn && index === -1)
        {
          this.model.features.push(alias);
        }
      }
    }
  }
</script>
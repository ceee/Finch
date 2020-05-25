<template>
  <ui-form ref="form" class="page" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-header-bar :title="title" :on-back="onBack">
      <ui-dropdown>
        <template v-slot:button>
          <ui-button type="white" label="Actions" caret="down" />
        </template>
        <ui-dropdown-list v-model="actions" :action="actionSelected" />
      </ui-dropdown>
      <ui-button type="white" label="Preview" icon="fth-eye" />
      <ui-button :submit="true" label="Save" />
    </ui-header-bar>

     <ui-editor v-if="!loading" :config="renderer" v-model="model" />
  </ui-form>
</template>


<script>
  import UiEditor from 'zero/editor/editor';
  import PagesApi from 'zero/resources/pages';

  export default {

    props: ['id', 'type'],

    components: { UiEditor },

    data: () => ({
      loading: true,
      renderer: null,
      actions: [],
      model: {
        name: null,
        options: {
          hideInNavigation: false
        },
        link: null
      }
    }),


    computed: {
      isCreate()
      {
        return this.$route.name === 'page-create';
      },
      title()
      {
        return this.isCreate ? 'Create new page' : 'Page ' + this.id;
      }
    },


    watch: {
      '$route': function ()
      {
        this.initialize();
      }
    },


    mounted()
    {
      this.initialize();
    },


    methods: {

      initialize()
      {
        console.info(this.id, this.type);
      },

      actionSelected(item, dropdown)
      {
        dropdown.hide();
      },

      onBack()
      {
        this.$router.go(-1);
      },

      onLoad(form)
      {
        form.load(PagesApi.getById(this.$route.params.id)).then(response =>
        {
          this.renderer = 'debug.' + response.entity.pageTypeAlias + 'page';
          this.model = response.entity;
          this.loading = false;
        });
      },


      onSubmit(form)
      {
        //this.fullModel.model = this.model;

        console.info(JSON.parse(JSON.stringify(this.model)));
        //form.handle(PagesApi.save(this.model)).then(response =>
        //{
        //  console.info(response);
        //});
      },

    }
  }
</script>
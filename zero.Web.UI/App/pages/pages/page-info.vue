<template>
  <div class="page-editor-info ui-view-box has-sidebar">
    <div>
      <div class="ui-box">
        <h3 class="ui-headline" v-localize="'Links'"></h3>
        <div>
        <a v-if="value.route" class="ui-button type-light" :href="value.route.url" target="_blank">
          <span class="ui-button-text">Open page</span>
        </a>
        <br /><br /><br /><br /></div>
        <h3 class="ui-headline" v-localize="'@revisions.label'"></h3>
        <ui-revisions v-if="!isCreate" :get="getRevisions" />
      </div>
    </div>
    <div class="ui-view-box-aside editor-infos">
      <div class="ui-box editor-active-toggle" :class="{'is-active': value.isActive }">
        <ui-property label="@ui.active" :is-text="true" class="is-toggle">
          <ui-toggle v-model="value.isActive" class="is-primary" :disabled="disabled" />
        </ui-property>
        <ui-property label="@page.schedule.label" :is-text="true">
          <ui-daterangepicker :value="{ from: value.publishDate, to: value.unpublishDate }" @input="onRangeChange" :class="{ 'is-primary': value.publishDate || value.unpublishDate }" :disabled="disabled" />
        </ui-property>
      </div>
      <div class="ui-box is-light">
        <ui-property v-if="!isCreate" label="@ui.id" :is-text="true">
          {{value.id}}
        </ui-property>
        <ui-property v-if="!isCreate" label="@ui.createdDate" :is-text="true">
          <ui-date v-model="value.createdDate" />
        </ui-property>
        <ui-property label="@page.type" :is-text="true" v-if="pageType">
          <i :class="pageType.icon"></i> &nbsp;{{pageType.name}}
        </ui-property>
      </div>
    </div>
  </div>
</template>


<script>
  import PagesApi from 'zero/resources/pages';

  export default {
    props: {
      value: {
        type: [ Object, Array ]
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      pageType: null
    }),

    computed: {
      isCreate()
      {
        return !this.value.id;
      }
    },

    mounted()
    {
      PagesApi.getPageType(this.value.pageTypeAlias).then(pageType =>
      {
        this.pageType = pageType;
      });
    },


    methods: {

      getRevisions(page)
      {
        return PagesApi.getRevisions(this.value.id, page);
      },

      onRangeChange(value)
      {
        this.value.publishDate = value.from;
        this.value.unpublishDate = value.to;
      },

    }
  }
</script>
<template>
  <div class="page-editor-info ui-view-box has-sidebar">
    <div>
      <div class="ui-box"></div>
      <div class="ui-box">
        <h3 class="ui-headline">Revisions</h3>
        <ui-revisions v-model="revisions" />
      </div>
    </div>
    <div class="ui-view-box-aside editor-infos">
      <div class="ui-box editor-active-toggle" :class="{'is-active': value.isActive }">
        <ui-property label="@ui.active" :is-text="true" class="is-toggle">
          <ui-toggle v-model="value.isActive" class="is-primary" />
        </ui-property>
        <ui-property label="@page.schedule.label" :is-text="true">
          <ui-daterangepicker :value="{ from: value.publishDate, to: value.unpublishDate }" @input="onRangeChange" />
        </ui-property>
      </div>
      <div class="ui-box">
        <ui-property v-if="!isCreate" label="@ui.id" :is-text="true">
          {{value.id}}
        </ui-property>
        <ui-property v-if="!isCreate" label="@ui.createdDate" :is-text="true">
          <ui-date v-model="value.createdDate" />
        </ui-property>
        <ui-property label="Type" :is-text="true" v-if="pageType">
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
      }
    },

    data: () => ({
      pageType: null,
      revisions: {
        totalItems: 0,
        items: []
      }
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

      if (!this.isCreate)
      {
        PagesApi.getRevisions(this.value.id).then(response =>
        {
          this.revisions = response;
        });
      }
    },


    methods: {

      onRangeChange(value)
      {
        this.value.publishDate = value.from;
        this.value.unpublishDate = value.to;
      },

    }
  }
</script>
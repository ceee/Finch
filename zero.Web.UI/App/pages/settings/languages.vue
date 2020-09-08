<template>
  <div class="languages">
    <ui-header-bar title="@language.list" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-add-button :route="createRoute" :decision="false" :shared="true" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import LanguagesApi from 'zero/resources/languages.js';

  export default {
    data: () => ({
      createRoute: zero.alias.sections.settings + '-' + zero.alias.settings.languages + '-create',
      tableConfig: zero.renderers.language.list
    }),

    created()
    {
      this.tableConfig.items = LanguagesApi.getAll;
    }
  }
</script>
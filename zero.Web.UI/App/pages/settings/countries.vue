<template>
  <div class="countries">
    <ui-header-bar title="@country.list" :count="count" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-add-button :route="createRoute" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" v-model="tableConfig" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  import CountriesApi from 'zero/resources/countries.js';

  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.sections.settings + '-' + zero.alias.settings.countries + '-create',
      tableConfig: zero.renderers.country.list
    }),

    created()
    {
      this.tableConfig.items = CountriesApi.getAll;
    }
  }
</script>
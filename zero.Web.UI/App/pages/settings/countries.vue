<template>
  <div class="countries">
    <x-header-bar title="@country.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <my-add-button :route="createRoute" />
    </x-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="countries" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  import { headerBar as xHeaderBar, addButton as xAddButton } from './dynamic.js';

  console.info(xHeaderBar, xAddButton);

  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.settings.countries + '-create'
    }),

    components: { xHeaderBar, xAddButton }
  }
</script>
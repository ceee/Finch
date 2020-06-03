<template>
  <div class="countries">
    <ui-header-bar title="@country.list" :back-button="true">
      <ui-table-filter v-model="tableConfig" :selection="selection" :select-actions="selectActions" />
      <ui-button label="@ui.add" icon="fth-plus" @click="add" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" v-model="tableConfig" @select="onSelect" />
    </div>
  </div>
</template>


<script>
  import CountriesApi from 'zero/resources/countries.js';

  const baseRoute = zero.alias.sections.settings + '-' + zero.alias.settings.countries;

  export default {
    data: () => ({
      tableConfig: {},
      selection: [],
      selectActions: []
    }),

    created()
    {
      this.selectActions.push({
        name: 'Delete',
        icon: 'fth-trash'
      });

      this.tableConfig = {
        labelPrefix: '@country.fields.',
        allowOrder: false,
        search: null,
        selectable: true,
        columns: {
          flag: {
            label: '',
            as: 'html',
            render: item => `<i class="flag flag-${item.code.toLowerCase()}"></i>`,
            width: 62
          },
          name: {
            label: '@ui.name',
            as: 'text',
            bold: true,
            link: item =>
            {
              return {
                name: baseRoute + '-edit',
                params: { id: item.id }
              };
            }
          },
          code: 'text',
          isPreferred: {
            as: 'bool',
            width: 200
          },
          isActive: {
            as: 'bool',
            label: '@ui.active',
            width: 200
          }
        },
        items: CountriesApi.getAll
      };
    },


    methods: {
      goBack()
      {
        this.$router.go(-1);
      },

      onSelect(items)
      {
        this.selection = items;
      },

      add()
      {
        this.$router.push({ name: baseRoute + '-create' });
      }
    }
  }
</script>
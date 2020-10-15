let link = item =>
{
  return {
    name: zero.alias.sections.settings + '-' + zero.alias.settings.translations + '-edit',
    params: { id: item.id }
  };
};

export default {
  alias: 'translation',

  list: {
    labelPrefix: '@translation.fields.',
    order: {
      enabled: false
    },
    columns: {
      key: {
        as: 'text',
        shared: true,
        link: link
      },
      value: {
        as: 'text',
        link: link
      }
    },
  }
};
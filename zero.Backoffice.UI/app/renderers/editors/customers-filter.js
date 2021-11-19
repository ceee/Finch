
import ListFilter from 'zero/core/list-filter.js';

const filter = new ListFilter('country', '@country.fields.');

filter.template = {
  date: {
    from: null,
    to: null
  },
  countryId: null,
  turnover: {
    from: null,
    to: null
  },
  search: null
};

filter.for('date').dateRangePicker({ inline: true }).preview({
  hasValue: x => x.from || x.to,
  icon: 'fth-calendar',
  render: Localization.localize(!x.from && !x.to ? null : (x.from && !x.to ? '@ui.date.x' : (!x.from && x.to ? '@ui.date.y' : '@ui.date.xtoy')), { tokens: { x: Strings.date(x.from), y: Strings.date(x.to) } })
});

filter.for('countryId').countryPicker().preview({
  hasValue: x => !!x,
  icon: 'fth-globe',
  render: x => x != null ? 'Selected' : null 
});

filter.for('turnover').custom(null /* '@shop/editor/pricerange.vue' */).preview({
  hasValue: x => x.from || x.to,
  icon: 'fth-dollar-sign',
  render: Localization.localize(!x.from && !x.to ? null : (x.from && !x.to ? '@ui.price.x' : (!x.from && x.to ? '@ui.price.y' : '@ui.price.xtoy')), { tokens: { x: Strings.currency(x.from), y: Strings.currency(x.to) } })
});

filter.for('search').text(null, 'Enter a search term...').preview({
  hasValue: x => !!x,
  icon: 'fth-search',
  preview: x => x
});

return filter;

import List from 'zero/core/list.ts';
import RecycleBinApi from 'zero/api/recycle-bin.js';
import Overlay from 'zero/helpers/overlay.js';
import RecycleBinActionsOverlay from 'zero/pages/pages/recyclebin/recyclebin-actions.vue';
import EventHub from 'zero/helpers/eventhub.js';

const list = new List('recyclebin');
const prefix = '@recyclebin.fields.';

list.templateLabel = x => prefix + x;
list.onClick = item =>
{
  Overlay.open({
    component: RecycleBinActionsOverlay,
    display: 'editor',
    model: item
  }).then(value =>
  {
    const deleted = !!value.deleted;

    // go to restored page
    if (!deleted)
    {
      EventHub.$emit('page.update');
      this.$router.push({
        name: 'page',
        params: { id: item.originalId }
      });
    }
    // reload recycle bin
    else
    {
      this.$refs.table.update();
    }
  });
}; //zero.alias.settings.countries + '-edit';

list.onFetch(filter =>
{
  filter.group = 'pages';
  return RecycleBinApi.getByQuery(filter);
});

list.column('name').name();
list.column('originalId').text();
list.column('createdDate').created();

export default list;
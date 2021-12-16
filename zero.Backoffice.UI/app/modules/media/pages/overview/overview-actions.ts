
import api from '../../api';
import * as overlays from '../../../../services/overlay';
import * as notifications from '../../../../services/notification';


export default {

  remove(items: any[])
  {
    console.info({ action: 'remove', items });
  },

  async move(items: any[])
  {
    const result = await overlays.open({
      component: () => import('../../overlays/move.vue'),
      display: 'editor',
      width: 560,
      model: {
        parentId: items[0].parentId,
        name: items.length + ' items',
        items: items
      }
    });

    if (result.value)
    {
      notifications.success('Successfully moved', `Moved ${items.length} items to new location`);
      return items.map(x => x.id);
    }
  }
};
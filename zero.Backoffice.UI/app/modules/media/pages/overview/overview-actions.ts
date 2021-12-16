
import api from '../../api';
import * as overlays from '../../../../services/overlay';
import * as notifications from '../../../../services/notification';
import { localize } from '../../../../services/localization';


export default {

  async remove(items: any[])
  {
    const ids = items.map(x => x.id);
    const response = await api.bulkGetDescendants(ids);

    const result = await overlays.confirmDelete("@media.deleteoverlay.title", '@media.deleteoverlay.folder_text', localize('@media.deleteoverlay.warning', {
      tokens: {
        folders: response.data.folders,
        files: response.data.files
      } as Record<string, string>
    }));

    if (result.eventType === 'confirm')
    {
      const confirmResult = await overlays.confirmDelete(undefined, undefined, undefined, {
        model: {
          title: "@media.deleteoverlay.start.title",
          warning: null,
          text: null,
          confirmLabel: "@media.deleteoverlay.start.button",
          confirmType: 'danger',
          closeLabel: '@deleteoverlay.close'
        }
      });
      result.close();
      console.info(confirmResult);
    }

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
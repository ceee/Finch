
import api from '../../api';
import * as overlays from '../../../../services/overlay';


export class Actions
{
  constructor()
  {

  }


  remove(items: any[])
  {
    console.info({ action: 'remove', items });
  }


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

    console.info(result);
   
  }
};
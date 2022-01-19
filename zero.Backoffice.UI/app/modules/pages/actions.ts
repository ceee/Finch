
import api from './api';
import * as overlays from '../../services/overlay';
import * as notifications from '../../services/notification';
import { localize } from '../../services/localization';
import { Router } from 'vue-router';
import eventHub from '../../services/eventhub';

export default {

  async create(router: Router, parent: object)
  {
    const result = await overlays.open({
      component: () => import('./overlays/create.vue'),
      width: 480,
      model: {
        parent
      }
    });

    if (result.value)
    {
      router.push({
        name: 'pages-create',
        params: {
          flavor: result.value.alias,
          parent: parent ? parent.id : undefined
        }
      });
    }
  },


  async move(model: object)
  {
    const result = await overlays.open({
      component: () => import('./overlays/move.vue'),
      display: 'editor',
      width: 480,
      model: model
    });

    if (result.eventType == 'confirm')
    {
      eventHub.emit('page.update', { action: 'move', model: result.value });
      eventHub.emit('page.move', result.value);
    }
  },


  async copy(router: Router, model: object)
  {
    const result = await overlays.open({
      component: () => import('./overlays/copy.vue'),
      display: 'editor',
      width: 480,
      model: model
    });

    if (result.eventType == 'confirm')
    {
      router.push({
        name: 'pages-edit',
        params: {
          id: result.value.id
        }
      });
      eventHub.emit('page.update', { action: 'copy', model: result.value });
      eventHub.emit('page.copy', result.value);
    }
  },


  async sort(model: object)
  {
    const result = await overlays.open({
      component: () => import('./overlays/sort.vue'),
      display: 'editor',
      width: 480,
      model: model
    });

    if (result.eventType == 'confirm')
    {
      eventHub.emit('page.update', { action: 'sort', model: result.value });
      eventHub.emit('page.sort', result.value);
    }
  },


  async remove(model: object)
  {
    const response = await api.getDependencies(model.id);

    const result = await overlays.confirmDelete("@page.deleteoverlay.title", '@page.deleteoverlay.text', localize('@page.deleteoverlay.warning', {
      tokens: {
        pages: response.data.pages
      }
    }));

    if (result.eventType == 'confirm')
    {
      result.value.state('loading');

      const apiresult = await api.delete(model.id);

      result.value.state('success');
      result.value.close();

      eventHub.emit('page.update', { action: 'delete', model: apiresult.value });
      eventHub.emit('page.delete', apiresult.data);
    }
  }

};